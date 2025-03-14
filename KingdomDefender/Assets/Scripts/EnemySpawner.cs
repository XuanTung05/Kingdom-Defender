using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Button startButton;
    [SerializeField] private Text countdownText;
    [SerializeField] private Text waveCounterText;

    [Header("Attributes")]
    [SerializeField] private float enemyPerSec = 0.5f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();

    [Header("Wave Settings")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private int totalWaves = 7;

    [Header("Main House Settings")]
    [SerializeField] private BaseHealth baseHealth;
    [SerializeField] private Image[] starImages;
    [SerializeField] private Sprite grayStar; // Gray star image
    [SerializeField] private Sprite goldStar; // Gold star image

    [Header("Victory Screen")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private int currentWaveIndex = 0;
    private int enemyAlive;
    private int enemyLeftToSpawn;
    private bool isSpawning = false;
    private bool waveCountdownRunning = false;

    private float timeSinceLastSpawn;
    private float timeRemaining;

    private void Awake()
    {
        if (waves.Length > 0)
        {
            timeRemaining = waves[currentWaveIndex].timeUntilNextWave;
        }
    }

    private void Start()
    {
        startButton.onClick.AddListener(StartWaveSequence);
        ShowStartButton();
        UpdateWaveCounterText();
        UpdateCountdownText(timeRemaining);

        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        victoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (isSpawning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / enemyPerSec) && enemyLeftToSpawn > 0)
            {
                SpawnEnemy();
                enemyLeftToSpawn--;
                enemyAlive++;
                timeSinceLastSpawn = 0f;
            }

            if (enemyAlive == 0 && enemyLeftToSpawn == 0 && !waveCountdownRunning)
            {
                EndWave();
            }
        }
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;

        if (currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            StartWaveCountdown();
        }
        else
        {
            StartCoroutine(ShowVictoryScreenAfterDelay(3f));
            UnlockNewLevel();
        }
    }

    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }

    private IEnumerator ShowVictoryScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0; // Pause the game
        ShowVictoryScreen();
    }

    private void StartNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            enemyLeftToSpawn = currentWave.enemyCounts[0];
            timeRemaining = currentWave.timeUntilNextWave;
            UpdateWaveCounterText();
            UpdateCountdownText(timeRemaining);
            isSpawning = true;

            StopCoroutine("WaveDurationCountdown");
            waveCountdownRunning = false;

            StartCoroutine(WaveDurationCountdown());
        }
    }

    private void StartWaveCountdown()
    {
        if (!waveCountdownRunning)
        {
            waveCountdownRunning = true;
            StartCoroutine(CountdownCoroutine());
        }
    }

    private IEnumerator CountdownCoroutine()
    {
        while (timeRemaining > 0)
        {
            UpdateCountdownText(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timeRemaining = 0;
        UpdateCountdownText(timeRemaining);
        waveCountdownRunning = false;

        StartNextWave();
    }

    private IEnumerator WaveDurationCountdown()
    {
        while (timeRemaining > 0)
        {
            UpdateCountdownText(timeRemaining);
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        timeRemaining = 0;
        UpdateCountdownText(timeRemaining);

        EndWave();
    }

    public void StartWaveSequence()
    {
        startButton.gameObject.SetActive(false);
        StartNextWave();
    }

    private void EnemyDestroy()
    {
        enemyAlive--;
    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);
    }

    private void UpdateCountdownText(float timeRemaining)
    {
        countdownText.text = $"Time left: {Mathf.CeilToInt(timeRemaining)}s";
    }

    private void UpdateWaveCounterText()
    {
        waveCounterText.text = $"Wave: {currentWaveIndex + 1}/{totalWaves}";
    }

    private void ShowVictoryScreen()
    {
        victoryPanel.SetActive(true);

        int stars = CalculateStars(baseHealth.currentHeart);
        UpdateStarsDisplay(stars);

        PlayerPrefs.SetInt("CurrentStars", stars);
        PlayerPrefs.Save();
    }

    private int CalculateStars(int remainingHealth)
    {
        if (remainingHealth >= 15)
        {
            return 3;
        }
        else if (remainingHealth >= 13)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    private void UpdateStarsDisplay(int stars)
    {
        for (int i = 0; i < starImages.Length; i++)
        {
            if (i < stars)
            {
                starImages[i].sprite = goldStar; // Set to gold star
            }
            else
            {
                starImages[i].sprite = grayStar; // Set to gray star
            }
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int[] enemyCounts;
        public float timeUntilNextWave;
    }

    private void ShowStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        Time.timeScale = 1; // Unpause the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1; // Unpause the game
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level Menu");
    }
}

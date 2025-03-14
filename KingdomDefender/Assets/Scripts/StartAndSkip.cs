using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAndSkip : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;

    public void StartWaveBut()
    {
        spawner.StartWaveSequence();
    }
    //public void SkipWaveBut()
    //{
    //    spawner.SkipWave();
    //}
}

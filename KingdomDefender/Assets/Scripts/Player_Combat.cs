//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Player_Combat : MonoBehaviour
//{
//    [SerializeField] private Animator fightAnimation;

//    public Transform attackPoint;
//    public float attackRange = 0.5f;
//    [SerializeField] private float attackCD = 0.5f;
//    private float coolDown;
//    private Player_Ctrl playerMovement;

//    public LayerMask enemies;
//    //Player player;

//    private string currentAni;
//    // Start is called before the first frame update
//    void Start()
//    {
//        if (Input.GetKeyDown(KeyCode.Q)  && coolDown > attackCD && playerMovement.canAttack())
//            Attack();

//        coolDown += Time.deltaTime;
//    }
//    // Update is called once per frame
//    void Update()
//    {
        
//    }

//    private void Attack()
//    {
//        ChangeAnimation("player1_atk");
//        coolDown = 0;
//    }
//    private void ChangeAnimation(string AniName)
//    {
//        if (currentAni != AniName)
//        {
//            fightAnimation.ResetTrigger(AniName);
//            currentAni = AniName;
//            fightAnimation.SetTrigger(currentAni);
//        }
//    }
//}

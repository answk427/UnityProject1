using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAttack : MonoBehaviour
{
    private Transform playerTr; //공격할 대상의 트랜스폼
    private Transform tr; //본인의 트랜스폼
    private Animator anim;
    private NavMeshAgent nav;
    public bool isAttack = false; //true일때만 공격하게하는 bool 변수
    
    public float attackSpeed = 2.0f;
    private float nextTime; //다음 공격시간을 저장하는 변수(현재시간 + attackSpeed)
    private float damping = 2.0f; //회전계수

    private readonly int attackHash = Animator.StringToHash("isAttack");

    
    private float attackDist = 3.0f; //공격 사정거리
    private float attackAngle = 30.0f;
    private Info enemyInfo;
    private Info playerInfo; 
    
    


    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("PLAYER");
        playerTr = player.GetComponent<Transform>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        nextTime = Time.time + attackSpeed;

        enemyInfo = GetComponent<Info>();
        playerInfo = playerTr.GetComponent<Info>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isAttack)
        {
            nav.isStopped = true;

            if (Time.time >= nextTime) 
            {

                Fire();
            }

            Vector3 dir = playerTr.position - tr.position;
            
            
            Quaternion rot = Quaternion.LookRotation(playerTr.position - tr.position); //플레이어 위치까지의 회전각도
            
           
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);
        }
         
            
    }

    void Fire()
    {
        anim.SetTrigger(attackHash);
        
        nextTime = Time.time + attackSpeed;
    }

    void Attack() //animation event로 추가
    {
        Vector3 dir = playerTr.position - tr.position;
                
        if (dir.magnitude < attackDist && 
            Vector3.Angle(tr.forward,dir.normalized) <= 0.5f * attackAngle) //공격 사정거리 이내에 있으면
            playerInfo.stat.Damaged(enemyInfo.stat.ATK);
    }

    
}

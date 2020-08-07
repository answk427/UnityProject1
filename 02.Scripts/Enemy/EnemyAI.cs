using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum State { NORMAL, DIE, HUNT, ATTACK };
    public Transform target; //target의 트랜스폼
    private Transform tr; //enemy의 트랜스폼

    State state = State.NORMAL;
    public bool isDie = false;

    public float attackDist = 3.0f; //공격범위
    public float huntDist = 6.0f; //추적사정거리

    private EnemyMove move; //EnemyMove 스크립트
    private EnemyAttack attack; //공격을 위한 스크립트
    private EnemyDie die; //사망처리 위한 스크립트
    private WaitForSeconds ws;

    private Animator anim;

    private readonly int moveHash = Animator.StringToHash("IsMove");
    private readonly int speedHash = Animator.StringToHash("Speed");


    private void Awake()
    {
        var obj = GameObject.FindGameObjectWithTag("PLAYER");
        target = obj.GetComponent<Transform>(); //플레이어의 트랜스폼
        tr = GetComponent<Transform>(); //Enemy의 트랜스폼
        attack = GetComponent<EnemyAttack>();

        move = GetComponent<EnemyMove>(); //EnemyMove 스크립트
        die = GetComponent<EnemyDie>(); //EnemyDie 스크립트
        ws = new WaitForSeconds(0.3f);
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
   
    // Update is called once per frame
    void Update()
    {
        anim.SetFloat(speedHash, move.speed); //속도에따라 걷기,뛰기 조절
    }

    IEnumerator CheckState()
    {
        while (!isDie) 
        {
            
            if (state == State.DIE)
                yield break;

            float dist = (target.position - tr.position).sqrMagnitude; //플레이어와의 거리

            if (dist <= attackDist * attackDist) //공격 범위내로 들어옴.
                state = State.ATTACK;
            
            else if (dist <= huntDist * huntDist) //추적 거리내로 들어옴.
                state = State.HUNT;
            else
                state = State.NORMAL;

            yield return ws;
        }

        state = State.DIE; //죽으면 루프 빠져나오면서 DIE 상태로 변경
    }

    IEnumerator Action()
    {
        if(isDie)
            Die(); //isDie가 true일 경우 Die함수 호출

        while (!isDie)
        {
            yield return ws;
            switch (state)
            {
                case State.DIE:
                    attack.isAttack = false;
                    
                    Die();
                    yield break;
                case State.ATTACK:
                    anim.SetBool(moveHash, false);
                    if(attack.isAttack == false)
                        attack.isAttack = true;
                    break;
                case State.HUNT:
                    move.IsHunt = target.position; //플레이어 위치 전달
                    attack.isAttack = false;
                    anim.SetBool(moveHash, true);
                    break;
                case State.NORMAL:
                    move.Normal = true;
                    attack.isAttack = false;
                    anim.SetBool(moveHash, true);
                    break;
            }
            
        }

    }

    void Die()
    {
        attack.isAttack = false;
        move.IsDie = true;
        die.ActionDie();
    }
    
}

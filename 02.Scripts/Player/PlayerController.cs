using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    
    //public DynamicJoystick dyn;

    private Animator anim;
    private Transform tr;
    private bool isDie = false;

    private readonly int attack1Trigger = Animator.StringToHash("Attack1Trigger"); //공격 트리거
    private readonly int attack2Trigger = Animator.StringToHash("Attack2Trigger");
    private readonly int rollTrigger = Animator.StringToHash("RollTrigger"); //구르기 트리거
    
    private int fireHash1, fireHash2; //공격 애니메이션의 해쉬값
    private int currState; //현재 애니메이션
    private float h = 0.0f; //horizontal 좌우입력값
    private float v = 0.0f; //vertical 앞뒤입력값

    public float moveSpeed = 3.0f;
    public float move_attack = 0.01f; //공격할때는 x배의 거리만 이동
    public float rotSpeed = 1.0f;

    private float attackAngle = 140.0f; //공격의 범위 각도
    [SerializeField]private float attackRange = 3.0f; //공격 사정거리
    private int layerMask; //공격시 ENEMY 감지를 위한 레이어마스크

    private bool isRoll = false; //구르기 중인지 판별
    public float rollDistance = 5.0f; //한번 구를때 이동하는 거리
    
    float rollPerSec; //구를때 1초동안 움직이는 거리
    float rollLength; //구르기 애니메이션이 재생되는 시간
    private Vector3 rollDir; //구르는 방향
    Info playerInfo;

    private AudioSource audioSource;
    
    void Awake()
    {
        PlayerStat.HPchange += PlayerDie;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();
        fireHash1 = Animator.StringToHash("Base Layer.Attack");
        fireHash2 = Animator.StringToHash("Base Layer.Attack2");
        
        layerMask = LayerMask.NameToLayer("ENEMY");

        RuntimeAnimatorController runAnim = anim.runtimeAnimatorController;
        
        for(int i=0; i<runAnim.animationClips.Length; i++)
        {
            if (runAnim.animationClips[i].name == "2Hand-Sword-Roll-Forward")
            {
                //roll 클립의 재생시간 / animator state의 재생속도
                rollLength = runAnim.animationClips[i].length / anim.GetFloat("RollSpeed");
                //구르기로 움직이는 거리 / 구르기 재생시간
                rollPerSec = rollDistance / rollLength;
            }
        }
        playerInfo = GetComponent<Info>();
        audioSource = GetComponent<AudioSource>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
            return;
        
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        
        //h = dyn.Horizontal;
        //v = dyn.Vertical;

        currState = anim.GetCurrentAnimatorStateInfo(0).fullPathHash; //현재 애니메이션 상태
        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h); //벡터의 합으로 계산된 방향
                  
        
        if (Input.GetButtonDown("Roll") && !isRoll && playerInfo.stat.MP >= 5) //구르기 버튼이 눌림
        {
            isRoll = true; 
            anim.SetTrigger(rollTrigger);
            if (h != 0 || v != 0)
                rollDir = moveDir;
            else //방향값이 없으면 뒤로구르기
                rollDir = -tr.forward;

            playerInfo.stat.MP -= 5;

        }
        else if (Input.GetButtonDown("FireButton")) //공격버튼 입력 감지
        {
                      
            if (currState == fireHash1) //공격모션1 실행중일때
            {
                anim.SetTrigger(attack2Trigger); //공격모션2 실행
            }
            else
            {
                anim.SetTrigger(attack1Trigger); //공격모션1 실행
                
            }
        }


        

        if (currState!=fireHash1 && currState!=fireHash2 && !isRoll) //공격모션중이 아닐때 달리기모션 실행
        {
            tr.LookAt(tr.position + moveDir);
            
            if (v >= 0.1 || v<=-0.1 || h>=0.1 || h<=-0.1)
                anim.SetBool("isRun", true);
            else
                anim.SetBool("isRun", false);

            tr.Translate(moveDir.normalized * Time.deltaTime * moveSpeed, Space.World); //월드좌표 기준으로 캐릭터 이동
        }
        else if(isRoll) 
        {
            tr.Translate(rollDir.normalized * Time.deltaTime * rollPerSec, Space.World); //구를때 일정거리만큼 이동
        }

        
    }

 
    public void PlayerAttack()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 moveDir = Vector3.forward * v + Vector3.right * h;
        tr.LookAt(tr.position + moveDir);

        
        Collider[] colls = Physics.OverlapSphere(tr.position, attackRange, 1<<layerMask); //플레이어 범위내에 있는 enemy 검출
        

        for(int i=0; i<colls.Length; i++)
        {
            Vector3 dir = (colls[i].transform.position - tr.position).normalized; //enemy를 바라보는 방향벡터

            if(Vector3.Angle(tr.forward, dir) <= attackAngle * 0.5f) //플레이어의 정면에서 공격범위각도에 있는지, 120도면 양쪽으로 60도
            {
               
                colls[i].GetComponent<EnemyDamaged>().Damaged(playerInfo.stat.ATK); //검출한 적의 Damaged스크립트를 이용해 피해를 줌
                audioSource.Play(); //적에 명중하면 사운드 재생
            }
            
        } 
    }
    
    public void ResetTrg(int i) //공격 애니메이션이 끝날때쯤 실행되는 이벤트
    {
        //광클로인해 잘못설정된 트리거를 초기화시킴
        if (i == 1)
        {
            
            anim.ResetTrigger(attack1Trigger);
            anim.ResetTrigger(attack2Trigger);
            
        }
        else if (i == 2)
        {
            
            anim.ResetTrigger(attack2Trigger);
            anim.ResetTrigger(attack1Trigger);
        }
       
    }

    public void ResetRoll() //구르기 애니메이션이 끝날때쯤 실행됨
    {
        isRoll = false;
    }

    
    //hp가 변동될때 검사하고 조건을 충족하면 실행됨. PlayerStat의 이벤트에 추가
    public void PlayerDie(int hp, int maxHp) 
    {
        if (hp <= 0 && isDie == false)
        {
            isDie = true;
            anim.SetBool("isDie", true);
            StartCoroutine(DeletePlayer());
            GameManager.instance.ShowGameOver();
        }
    }

    IEnumerator DeletePlayer() //사망 후 10초뒤에 캐릭터 비활성화
    {
        yield return new WaitForSeconds(10.0f);
        gameObject.SetActive(false);
    }

    
}
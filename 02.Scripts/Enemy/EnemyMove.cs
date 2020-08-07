using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;



public class EnemyMove : MonoBehaviour
{
    public List<Transform> wayPoints; //순찰 지점들 저장 List
    public int nextIdx; //다음 순찰지점의 Index
    public GameObject wayGroup; //포인트그룹 오브젝트 저장

    private Transform tr;
    private NavMeshAgent nav;
    private bool isDie; //죽으면 true
    private bool normal; //평소 순찰상태

    private Vector3 isHunt; //추적상태일시 플레이어의 위치를 받아옴.

    public float normalSpeed = 2.0f; //평소 순찰 속도
    public float huntSpeed = 3.5f; //추적 속도

    private float damping = 1.0f; //회전 계수
    
    public float speed
    {
        get { return nav.velocity.magnitude; }
    }
    
    public bool Normal
    {
        get
        {
            return normal;
        }
        set 
        {
            nav.speed = normalSpeed;
            normal = value;
            MoveWayPoint();
        }
    }
    public bool IsDie
    {
        get
        {
            return isDie;
        }
        set
        {
            isDie = value;
        }
    }

   
    public Vector3 IsHunt
    {
        get
        {
            return isHunt;
        }
        set
        {
            nav.speed = huntSpeed;
            normal = false;
            isHunt = value;
            Hunting(isHunt);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        nav = GetComponent<NavMeshAgent>();
        nav.autoBraking = false; //가까워지면 속도를 줄이는 속성 off
        nav.updateRotation = false; //내비게이터의 자동 회전기능 비활성화
        nextIdx = 0;

        if (wayGroup!=null)
        {
            //group의 자식들을 list에 추가
            wayGroup.GetComponentsInChildren<Transform>(wayPoints);
            
                
            //첫번째 항목 삭제 - 부모 오브젝트에 Transform이 있을경우 첫번째로 추가되기 때문.
            wayPoints.RemoveAt(0);
        }
        MoveWayPoint();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            nav.isStopped = true;
            return;
        }
            

        if (nav.isStopped == false)
        {
            //가려고 하는 방향의 velocity 벡터를 쿼터니언으로 변환
            Quaternion rot = Quaternion.LookRotation(nav.desiredVelocity);
            tr.rotation = Quaternion.Slerp(tr.rotation, rot, Time.deltaTime * damping);

        }

        if (!normal)
            return;

        //움직이고 있고 거의 도착 했을 때 다음 목표지점으로 이동
        if (nav.velocity.sqrMagnitude >= 0.2f * 0.2f && nav.remainingDistance <= 0.5f)
        {
            //nextIdx = ++nextIdx % wayPoints.Count;
            nextIdx = Random.Range(0, wayPoints.Count);
            MoveWayPoint();
        }
    }

    void MoveWayPoint()
    {
        if (nav.isPathStale) //경로가 유효하지 않음
            return;

        nav.destination = wayPoints[nextIdx].position; //다음 목적지 설정
        nav.isStopped = false;
    }

    void Hunting(Vector3 target)
    {
        if (nav.isPathStale)
            return;
        nav.destination = target;
        nav.isStopped = false;

    }
        
}

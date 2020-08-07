using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyDamaged : MonoBehaviour
{
    private Info info;
    public GameObject HpBar = null; //오브젝트풀로 할당받아 사용할 변수
    private EnemyHpBar bar; //EnemyHpBar 스크립트
    private CapsuleCollider coll; //높이계산을 위한 콜리더

    private const float HpTime = 3.0f; //hpBar의 최대 유지시간
    private float barTime = 0.0f; //지속되고있는 시간

    private WaitForSeconds ws = new WaitForSeconds(0.3f); //0.3초마다 시간계산
    

    private EnemyAI enemyAI; //죽을때 실행할 스크립트
    




    private void Start()
    {
        
        info = GetComponent<Info>();       
        coll = GetComponent<CapsuleCollider>();
        StartCoroutine("TimeCheck");
        enemyAI = GetComponent<EnemyAI>();
        
    }

    //0.3초마다 시간을체크, HpBar의 최대 지속시간을 넘으면 비활성화시킴
    IEnumerator TimeCheck()
    {
        while(true)
        {
            if(HpBar != null) 
            {
                if (barTime >= HpTime)
                {
                    HpBar.SetActive(false); //HpBar를 Enable 상태로 만듬
                    HpBar = null; //새로운 HpBar를 할당받기위해 null로 초기화
                    barTime = 0.0f;
                }
                else
                {
                    barTime += 0.3f;
                }
            }
            

            yield return ws;
                
        }
        
    }



    public void Damaged(int atk)
    {
        if(atk*2 > info.stat.DEF) //계산식이 방어력보다 높을때만 데미지를 받음
        {
           info.stat.HP -= atk - (int)(info.stat.DEF * 0.5); //플레이어의 공격력에서 Enemy의 방어력*0.5만큼 감소시킨 데미지를 받음
        }

        SetHpBar();

        if (info.stat.HP <= 0) //사망했을때
        {
            HpBar.SetActive(false);
            enemyAI.isDie = true;
        }

 
        
        
    }


    void SetHpBar()
    {
        if (HpBar != null) //HpBar를 반납하기전에 다시 공격받으면 barTime 0으로 초기화
        {
            barTime = 0.0f;
            if (info.stat.HP != 0)
                bar.SetHp((float)info.stat.HP / (float)info.stat.MAXHP);
            else
                bar.SetHp(0);
        }
        else
        {
            //싱글턴 오브젝트풀 매니저를통해 HpBar객체를 가져옴
            HpBar = PoolManager.instance.GetHpBar();
            bar = HpBar.GetComponent<EnemyHpBar>();
            bar.target = this.transform; //target을 현재 Enemy의 transform으로 설정
                                         //collider의 높이를 이용해 머리위에 hpBar 띄우도록 offset 조정
            bar.offset = Vector3.up * coll.height * transform.localScale.y + new Vector3(0, 0.5f, 0);
            HpBar.SetActive(true);

            if (info.stat.HP != 0)
                bar.SetHp((float)info.stat.HP / (float)info.stat.MAXHP);
            else
                bar.SetHp(0);
        }
        
    }
}

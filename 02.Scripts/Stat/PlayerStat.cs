using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStat : CommonStat
{
    
    public delegate void ChangeGauge(int hp, int maxHp);

    public static event ChangeGauge HPchange; //현재 HP가 바뀔 때 실행될 이벤트
    public static event ChangeGauge MPchange; //현재 MP가 바뀔 때 실행될 이벤트

    private bool immune = false; //무적상태를 판단
    private WaitForSeconds immuneTime = new WaitForSeconds(0.5f); //무적 지속시간
    

    public PlayerStat(int hp, int atk, int def, int mp) : base(hp, atk, def, mp)
    {
        
    }

    public override int HP //HP 프로퍼티 재정의
    {
        get
        {
            if (_HP > 0)
                return _HP;
            else
                return 0;
        }
        set
        {
            if (value > 0)
            {                
                _HP = value;
                HPchange(_HP, _MAXHP); //체력바 갱신
            }
            else
            {
                _HP = 0;
                HPchange(_HP, _MAXHP);
            }
                
        }

    }

    public override int MP //MP 프로퍼티 재정의
    {
        get
        {
            if (_HP > 0)
                return _MP;
            else
                return 0;
        }
        set
        {
            if (value > 0)
            {
                _MP = value;
                MPchange(_MP, _MAXMP); //마력바 갱신
            }
            else
            {
                _MP = 0;
                MPchange(_MP, _MAXMP);
            }

        }
    }

    public override void Damaged(int atk)
    {
        if(!immune) //무적이 아니면 데미지를 입음.
        {

            CoroutineManager.Start_Coroutine(ImmuneSet()); 
            int dmg = atk - (int)(_DEF * 0.5); //방어력의 반만큼 데미지감소    
                 
            if (dmg <= 0) //데미지 없음.
                return;

            HP -= dmg; //데미지만큼 hp계산
        }
            
    }

    IEnumerator ImmuneSet()
    {
        immune = true; //무적으로 설정
        yield return immuneTime; //무적 지속시간동안 기다림
        immune = false; //무적 해제
    }


}

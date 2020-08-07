using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStat 
{

    protected int _MAXHP; //최대체력
    protected int _HP; //현재체력
    protected int _ATK; // 공격력
    protected int _DEF; // 방어력
    protected int _MAXMP; //최대마력
    protected int _MP; //현재마력

    public CommonStat(int hp, int atk, int def, int mp)
    {
        _MAXHP = hp;
        HP = hp;
        ATK = atk;
        DEF = def;
        _MAXMP = mp;
        _MP = mp;
    }

    public int MAXHP
    {
        get
        {
            return _MAXHP;
        }
        set {_MAXHP = value; }
    }

    public virtual int HP 
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
                _HP = value;
            else
                _HP = 0;
        }
    }

    public virtual int MP
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
                
            }
            else
            {
                _MP = 0;
                
            }

        }
    }
    public int DEF { get => _DEF; set => _DEF = value; }
    public int ATK { get => _ATK; set => _ATK = value; }
    

    public void ShowStat()
    {
        Debug.Log("HP : " + HP + " ATK : " + ATK + " DEF : " + DEF + " MP : " + MP);
    }

    public virtual void Damaged(int atk) { }
}

 
using UnityEngine;

public class Info : MonoBehaviour
{
    public CommonStat stat;

    private void Start()
    {
        if (this.tag.Equals("ENEMY"))
            stat = new EnemyStat(100, 10, 10, 100); //체력, 공격력 ,방어력으로 Stat정보 생성
        else if (this.tag.Equals("PLAYER"))//플레이어일 경우
        {
            stat = new PlayerStat(200, 50, 10, 200); //체력, 공격력, 방어력, MP

        }
        else if (this.tag.Equals("BOSS"))
            stat = new EnemyStat(1000, 20, 10, 1000);
    }

    public void ShowInfo()
    {
        stat.ShowStat();
    }
    
}

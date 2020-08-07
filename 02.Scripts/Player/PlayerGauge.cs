using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGauge : MonoBehaviour
{
    UnityEngine.UI.Image img; 
    Text text;

    private void Awake()
    {
        if (name.Equals("HP"))
            PlayerStat.HPchange += this.SetAmount; //체력이 바뀔때마다 체력바 조절하기위한 이벤트에 추가
        else if (name.Equals("MP"))
            PlayerStat.MPchange += this.SetAmount; //마력 바뀔때..

        img = GetComponent<UnityEngine.UI.Image>();
        text = transform.GetChild(0).GetComponent<Text>();
    }
  
    public void SetAmount(int hp, int maxHp)
    {
        img.fillAmount = (float)hp / maxHp;
        text.text = hp + " / " + maxHp;
    }

    private void OnDisable()
    {
        if (name.Equals("HP"))
            PlayerStat.HPchange -= this.SetAmount; 
        else if (name.Equals("MP"))
            PlayerStat.MPchange -= this.SetAmount;
    }
}

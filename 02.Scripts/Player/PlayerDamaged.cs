using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamaged : MonoBehaviour
 {

    Info playerInfo; //플레이어의 공격력,체력등 정보를 가진 스크립트
    GameObject hp; //체력바 ui 오브젝트
    PlayerGauge redHp; //체력바의 조절을 위한 스크립트
    Text text; //체력을 텍스트로 표시하기 위한 컴포넌트


    void OnEnable()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = GetComponent<Info>();
        hp = GameObject.Find("PlayerHP");
        //redHp = hp.transform.GetChild(1).GetComponent<PlayerGauge>();
        text = hp.transform.GetChild(2).GetComponent<Text>();
    }
    

    
}

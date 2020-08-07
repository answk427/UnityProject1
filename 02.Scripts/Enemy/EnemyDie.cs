using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDie : MonoBehaviour
{
    private Animator anim; //Animator 변수
    private static readonly int animDie = Animator.StringToHash("isDie");
    private static readonly int animDieTrigger = Animator.StringToHash("isDieTrigger");

    private WaitForSeconds delTime = new WaitForSeconds(3.0f); //죽는 모션 후 사라지는 시간


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        
    }

    public void ActionDie()
    {
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        //죽는모션 실행
        anim.SetBool(animDie, true); 
        anim.SetTrigger(animDieTrigger);
        if(this.CompareTag("BOSS")) //보스가 죽었을 경우 게임을 승리함.
            GameManager.instance.Victory();
        yield return delTime; //일정시간 기다림
        gameObject.SetActive(false); //게임오브젝트 비활성화
    }

    private void OnDisable()
    {
        transform.localPosition = Vector3.zero;
    }


}

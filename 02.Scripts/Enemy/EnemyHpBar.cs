
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class EnemyHpBar : MonoBehaviour
{
    private RectTransform tr; //HpBar의 RectTransform
    public Transform target; //따라다닐 target의 RectTransform
    public Vector3 offset; //target 위로 일정 높이만큼 위치시키기 위한 변수

    private Canvas canvas; //최상위 UI 캔버스
    private RectTransform canvasTr; //UI 캔버스의 RectTransform

    private Camera uiCamera; //UI 카메라
    UnityEngine.UI.Image redHp; //남은 hp를 나타내는 붉은색 이미지


    private void OnDisable()
    {
        
        target = null;
        tr.localPosition = Vector3.zero;
        tr.localEulerAngles = Vector3.zero;
        SetHp(1);
    }
    private void OnEnable()
    {
        tr = GetComponent<RectTransform>();
                
        redHp = tr.GetChild(0).GetComponent<UnityEngine.UI.Image>();

    }



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        canvas = GameObject.Find("UI Canvas").GetComponent<Canvas>();
        
        canvasTr = canvas.GetComponent<RectTransform>();
        
        uiCamera = canvas.worldCamera;
        
    }

    private void LateUpdate()
    {
        //월드 좌표를 스크린 좌표로 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position + offset);

        //z값이 0보다 작은경우 카메라 뒤쪽(안보이는 부분)이므로 보정 해줌.
        if (screenPos.z < 0)
        {
            screenPos *= -1;    
        }
            

        Vector2 localPos = Vector2.zero;

        //스크린 좌표를 UI 캔버스 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTr, screenPos, uiCamera, out localPos);
        
        tr.localPosition = localPos;
    }

    public void SetHp(float hp)
    { 
        redHp.fillAmount = hp;
    }
}

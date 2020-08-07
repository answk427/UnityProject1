using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null; //싱글턴 접근하기 위한 변수
    public List<GameObject> hpPoolList; //오브젝트들을 넣어놓는 List
    public GameObject hpBarPrefab; //인스펙터뷰로 hpbar 프리팹 넣기위한 변수
    private int hpBarMax = 15; //오브젝트풀로 사용할 최대 갯수(hpBar)

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) //처음 object가 생성될 경우
            instance = this;
        else if (instance != this) //PoolManager가 붙은 오브젝트가 새로 생성될경우 삭제
            Destroy(this.gameObject);

        

        InitHpBarPool();
    }

    private void InitHpBarPool()
    {
        RectTransform ui = GameObject.Find("UI Canvas").GetComponent<RectTransform>();

        //오브젝트들을 모아 둘 빈 오브젝트 생성
        GameObject hpBarPool = new GameObject();
        hpBarPool.transform.SetParent(ui);

        //localPosition을 0으로 초기화하지 않으면 UI에서 그만큼 옮겨진 위치에 HpBar가 나타남.
        hpBarPool.transform.localPosition = Vector3.zero;
        
        //localScale을 1로 초기화하지 않으면 자식오브젝트들의 크기가 변하고 HpBar가 정상적인 자리에 위치하지 않음.
        hpBarPool.transform.localScale = Vector3.one; 
        hpBarPool.name = "hpBarPool";
        
        
        
        for(int i=0; i<hpBarMax; i++)
        {
            //hpBar를 최대갯수만큼 hpBarPool 오브젝트의 자식으로 생성
            GameObject hpBar = Instantiate<GameObject>(hpBarPrefab, hpBarPool.transform);
            hpBar.name = "hpBar_" + i.ToString("00");
            hpBar.SetActive(false); //사용하기 전엔 비활성화
            hpPoolList.Add(hpBar); //pool리스트에 추가
            
        }
    }

    public GameObject GetHpBar()
    {
        for(int i=0; i<hpBarMax; i++)
        {
            if (hpPoolList[i].activeSelf == false)
                return hpPoolList[i]; //비활성화 된 hpBar를 반환
        }

        return null;
    }
}

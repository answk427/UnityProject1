using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //싱글턴 변수
    public Canvas gameover; //gameover 캔버스
    public Canvas restartButton;
    
    

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

    }

    public void Victory()
    {
        Text text = gameover.GetComponentInChildren<Text>();
        text.color = Color.blue;
        text.text = "Stage Clear!";
        ShowGameOver();
    }
     

    public void ShowGameOver()
    {
        gameover.gameObject.SetActive(true); //캔버스 활성화
        
        StartCoroutine(AlphaUp());
        StartCoroutine(TextUp());
    }

    IEnumerator AlphaUp()
    {
        Image img = gameover.GetComponentInChildren<Image>();
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0.1f);

        while (img.color.a<1)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a+0.05f);
            
            yield return new WaitForSeconds(0.1f);
        }

        while (img.color.a > 0.5)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - 0.05f);

            yield return new WaitForSeconds(0.1f);
        }

    }

    IEnumerator TextUp()
    {
               
        Text text = gameover.GetComponentInChildren<Text>();
        text.fontSize = 0;

        while(text.fontSize <30)
        {
            text.fontSize += 1;
            yield return new WaitForSeconds(0.05f);
        }

        restartButton.gameObject.SetActive(true);
        
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Play", LoadSceneMode.Single);
    }
    
}

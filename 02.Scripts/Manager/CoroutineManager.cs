using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    IEnumerator enumerator = null;
     

    private void Update()
    {
        if(enumerator!=null)
        {
            if (enumerator.Current == null)
                Destroy(gameObject);
        }
    }

    private void Coroutine(IEnumerator e)
    {
        enumerator = e;
        StartCoroutine(e);
    }

    public static CoroutineManager Start_Coroutine(IEnumerator e)
    {
        GameObject obj = new GameObject("CoroutineManager");
        CoroutineManager mgr = obj.AddComponent<CoroutineManager>();
        if(mgr)
        {
            mgr.Coroutine(e);
        }
        return mgr;

    }

}

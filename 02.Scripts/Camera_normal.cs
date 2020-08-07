using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_normal : MonoBehaviour
{
    public Transform target;
    public float dist;
    public float height;
    private Transform tr;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        tr.position = target.position + (Vector3.up * height) - (Vector3.forward * dist);
       
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AguaMovimento : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
   
    }
    void Awake()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + Mathf.Sin(Time.time) * 0.0005f, transform.localPosition.z);
    }
}

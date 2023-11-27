using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class Airdrop : MonoBehaviour
{


    private bool aberto = false;
    private bool abriu = false;
    private float timer = 180;
    public GameObject caixacaindo;
    public GameObject caixa;
    public String[] items;
    public Rigidbody rb;
    public PhotonView pv;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>(); //pegar o objeto rigidbody
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!aberto&&transform.position.y < 10)
        {
            Debug.Log("Airdrop Abriu");
            abriu = true;
            aberto = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(abriu)
        {
            for(var i = 0; i<4; i++)
            {
                PhotonNetwork.Instantiate(items[Random.Range(0,3)], transform.position, Quaternion.Euler(Vector3.up * Random.Range(0, 360.0f)));
            }
            caixacaindo.SetActive(false);
            caixa.SetActive(true);
            rb.isKinematic = true;
            abriu = false;
        }
        if(aberto)
        {
            if (timer > 0) timer -= 60 * Time.deltaTime;
            if(timer<=0)
            {
                if(pv.IsMine) PhotonNetwork.Destroy(pv);
            }
        }
    }
}

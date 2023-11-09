using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MacaAnimal : MonoBehaviour
{

    [PunRPC]
    void SetAnimal(int idd)
    {
        animallist[idd].SetActive(true);
    }
    [PunRPC]
    void SetItem(int idd)
    {
        itemnec = idd;
        baloeslist[idd].SetActive(true);
    }
    [PunRPC]
    void Finalizar()
    {
        finalizado = true;
    }
    [PunRPC]
    void Setlado(string lados)
    {
        lado = lados;
        //spawner = spawners;
    }

    [PunRPC]
    void SetQuant()
    {
        quantiminus = true;
        //lado = lados;
        //spawner = spawners;
    }



    public PhotonView macapv;
    public bool atua = false;
    public GameObject spawner;
    public bool quantiminus = false;
    public float spd = 1;
    public string lado = "";
    public bool finalizado = false;
    public GameObject[] animallist;
    public GameObject[] baloeslist;
    public int itemnec = -1;
    // Start is called before the first frame update
    void Start()
    {
        macapv = GetComponent<PhotonView>();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            //setar animal
            var randomn = Random.Range(0, 3); //0 a 2
            animallist[randomn].SetActive(true);
            macapv.RPC("SetAnimal", RpcTarget.OthersBuffered, randomn);
            //setar item
            var randomn2 = Random.Range(0, 2); //0 a 1
            itemnec = randomn2;
            baloeslist[randomn2].SetActive(true);
            macapv.RPC("SetItem", RpcTarget.OthersBuffered, randomn2);
            if (lado == "esquerda")
            {
                transform.rotation = new Quaternion(0, 180, 0, 0); 
            }
            macapv.RPC("Setlado", RpcTarget.OthersBuffered, lado);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (quantiminus)
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                macapv.RequestOwnership();
                if(macapv.IsMine)
                {
                    spawner.GetComponent<GameSpawn>().quantion--;
                    PhotonNetwork.Destroy(macapv.GetComponent<PhotonView>());
                    quantiminus = false;
                }
            }
            else quantiminus = false;
        }
        else
        {
            if (lado == "direita")
            {
                if (finalizado)
                {
                    if (!atua)
                    {
                        macapv.RPC("Finalizar", RpcTarget.OthersBuffered);
                        atua = true;
                    }
                    baloeslist[itemnec].SetActive(false);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (-1.5f * Time.deltaTime));
                    if (transform.position.z < -20)
                    {
                        if (macapv.IsMine)
                        {
                            quantiminus = true;
                            macapv.RPC("SetQuant", RpcTarget.OthersBuffered);

                        }

                    }
                }
                else
                {
                    if (transform.position.z < -10)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (1.5f * Time.deltaTime));
                    }
                }
            }
            if (lado == "esquerda")
            {
                if (finalizado)
                {
                    if (!atua)
                    {
                        macapv.RPC("Finalizar", RpcTarget.OthersBuffered);
                        atua = true;
                    }
                    //macapv.RPC("Finalizar", RpcTarget.OthersBuffered);
                    baloeslist[itemnec].SetActive(false);
                    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (1.5f * Time.deltaTime));
                    if (transform.position.z > 17)
                    {
                        if (macapv.IsMine)
                        {
                            quantiminus = true;
                            macapv.RPC("SetQuant", RpcTarget.OthersBuffered);
                        }

                    }


                }
                else
                {
                    if (transform.position.z > 9)
                    {
                        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (-1.5f * Time.deltaTime));
                    }
                }
            }
        }
        //if (PhotonNetwork.LocalPlayer.IsMasterClient)
        //{
            
        //}
    }

   

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            if (other.gameObject.GetComponent<PlayerController>().segurando)
            {
                var itemspec = other.gameObject.GetComponent<PlayerController>().itemspec;
                //Debug.Log("item necessario: " + itemnec + " item na mao: " + itemspec.GetComponent<OwnerItem>().itemid);
                if (itemspec.GetComponent<OwnerItem>().itemid == itemnec)
                {
                    macapv.TransferOwnership(other.GetComponent<PlayerController>().player);
                    other.gameObject.GetComponent<PlayerController>().segurandocerto = true;
                    other.gameObject.GetComponent<PlayerController>().macacerta = this.gameObject;
                    //other.gameObject.GetComponent<PlayerController>().myPhotonView.RPC("macarpc", RpcTarget.OthersBuffered, true, this.gameObject);
                    /*
                    if (other.gameObject.GetComponent<PlayerController>().entregou==true)
                    {
                        Debug.Log("ajudame");
                        finalizado = true;
                        other.gameObject.GetComponent<PlayerController>().entregou = false;
                    }
                    */
                }
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            if (other.gameObject.GetComponent<PlayerController>().segurando)
            {
                var itemspec = other.gameObject.GetComponent<PlayerController>().itemspec;
                //Debug.Log("item necessario: " + itemnec + " item na mao: " + itemspec.GetComponent<OwnerItem>().itemid);
                if (itemspec.GetComponent<OwnerItem>().itemid == itemnec)
                {
                    other.gameObject.GetComponent<PlayerController>().segurandocerto = false;
                }
            }
        }
    }
}

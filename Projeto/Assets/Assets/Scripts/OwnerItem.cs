using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OwnerItem : MonoBehaviourPunCallbacks
{

    public int parentid = -1;
    public bool soltou = false;
    public bool newow = false;
    public bool segurando2 = false;
    public bool pegou = false;
    private int playerid;
    private Rigidbody rb;
    private Collider coll;
    private PhotonView parent;
    private GameObject parenthand;
    public PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        PV = GetComponent<PhotonView>();
        parentid = -1;
    }

    [PunRPC]
    void ChangeParentId(int idd)
    {
        parentid = idd;
        pegou = true;

    }
    [PunRPC]
    void RestartParentId(int idd)
    {
        parentid = idd;
        soltou = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!newow)
        {
            //>= 0 alguem pegou
            if (pegou)
            {
                rb.isKinematic = true;
                coll.isTrigger = true;
                parent = PhotonView.Find(parentid);
                parenthand = parent.GetComponent<PlayerController>().hand;
                transform.position = parenthand.transform.position;
                this.transform.SetParent(parenthand.transform);
                this.transform.rotation = new Quaternion(0, 0, 0, 0);
                playerid = GlobalVariables.Instance.myPlayer.GetComponent<PlayerController>().myId;
                //Debug.Log("Pegou - IdPai: " + parentid + " IdPlayer: " + playerid);
                if (parentid == playerid)
                {
                    //Debug.Log("Eu sou eu, mas eu sou dono do photoview?: " + PV.IsMine);
                    if (PV.IsMine) PV.RPC("ChangeParentId", RpcTarget.OthersBuffered, parentid);
                }
                //Debug.Log(parent.gameObject);
                //this.transform.SetParent(parent.gameObject.transform);
                pegou = false;
                segurando2 = true;
            }
            //-1 soltou
            if (soltou)
            {
                if (parentid != -1)
                {
                    if (segurando2)
                    {
                        playerid = GlobalVariables.Instance.myPlayer.GetComponent<PlayerController>().myId;
                        //Debug.Log("Soltou - IdPai: " + parentid + " IdPlayer: " + playerid);
                        if (parentid == playerid)
                        {
                            if (PV.IsMine) PV.RPC("RestartParentId", RpcTarget.OthersBuffered, parentid);
                        }
                        rb.isKinematic = false;
                        coll.isTrigger = false;
                        this.transform.SetParent(null);
                        soltou = false;
                        segurando2 = false;
                    }
                    else
                    {
                        Debug.Log("Erro: Segurando2 deu false");

                    }
                }
                else Debug.Log("Erro: Codigo do pai chegando -1");

            }
        }
        else
        {
            if (PV.IsMine) newow = false;
        }
       
    }

}

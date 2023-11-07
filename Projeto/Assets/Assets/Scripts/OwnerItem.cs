using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OwnerItem : MonoBehaviourPunCallbacks
{

    public int parentid = -2;
    public bool soltou = false;
    public bool segurando = false;
    public bool pegou = false;
    private int playerid;
    private Rigidbody rb;
    private Collider coll;
    private PhotonView parent;
    private GameObject parenthand;
    private PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        PV = GetComponent<PhotonView>();
        parentid = -2;
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
        //>= 0 alguem pegou
        if (pegou)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            parent = PhotonView.Find(parentid);
            parenthand = parent.GetComponent<PlayerController>().hand;
            transform.position = parenthand.transform.position;
            this.transform.SetParent(parenthand.transform);
            playerid = GlobalVariables.Instance.myPlayer.GetComponent<PlayerController>().myId;
            Debug.Log("Pegou - IdPai: " + parentid + " IdPlayer: " + playerid);
            if (parentid==playerid)
            {
                PV.RPC("ChangeParentId", RpcTarget.OthersBuffered, parentid);
            }
            //Debug.Log(parent.gameObject);
            //this.transform.SetParent(parent.gameObject.transform);
            pegou = false;
            segurando = true;
        }
        //-1 soltou
        if(soltou)
        {
            if (parentid != -1)
            {
                playerid = GlobalVariables.Instance.myPlayer.GetComponent<PlayerController>().myId;
                Debug.Log("Soltou - IdPai: " + parentid + " IdPlayer: " + playerid);
                if (parentid == playerid)
                {
                    PV.RPC("RestartParentId", RpcTarget.OthersBuffered, parentid);
                }
                rb.isKinematic = false;
                coll.isTrigger = false;
                this.transform.SetParent(null);
                soltou = false;
                segurando = false;
            }
            else Debug.Log("Erro: Codigo do pai chegando -1");
            
        }
       
    }

    public void ResquestOwner()
    {
        base.photonView.RequestOwnership();
    }
}

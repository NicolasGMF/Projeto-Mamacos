using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OwnerItem : MonoBehaviourPunCallbacks
{

    public int parentid = -2;
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
    }

    [PunRPC]
    void ChangeParentId(int idd)
    {
        parentid = idd;
    }
    [PunRPC]
    void RestartParentId()
    {
        parentid = -1;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        //>= 0 alguem pegou
        if (parentid>=0)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            parent = PhotonView.Find(parentid);
            parenthand = parent.GetComponent<PlayerController>().hand;
            transform.position = parenthand.transform.position; 
            if (PV.IsMine)
            {
                PV.RPC("ChangeParentId", RpcTarget.Others, parentid);
            }
            //Debug.Log(parent.gameObject);
            //this.transform.SetParent(parent.gameObject.transform);
            parentid = -3;
        }
        //-1 soltou
        if(parentid==-1)
        {
            if (PV.IsMine)
            {
                PV.RPC("ChangeParentId", RpcTarget.Others, -1);
            }
            rb.isKinematic = false;
            coll.isTrigger = false;
            this.transform.SetParent(null);
            parentid = -2;
        }
        //-2 nada
        if(parentid==-3)
        {
            //Debug.Log(parent.gameObject.transform.position);
            this.transform.position = parenthand.transform.position;
            this.transform.rotation = parenthand.transform.rotation;
        }
       
    }

    public void ResquestOwner()
    {
        base.photonView.RequestOwnership();
    }
}

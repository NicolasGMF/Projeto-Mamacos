using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class item : MonoBehaviourPunCallbacks
{
    public string itemname;
    public PhotonView myPhotonView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            var pos = transform.position;
            var rot = transform.rotation;
            var kinechan = gameObject.GetComponent<Rigidbody>();
            kinechan.isKinematic = true;
            GameObject objeto = PhotonNetwork.Instantiate(itemname, pos, rot);

        }
        gameObject.SetActive(false);
    }
}

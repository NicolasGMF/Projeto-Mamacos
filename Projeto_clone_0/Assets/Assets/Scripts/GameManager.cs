using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        
    }

    //--------------------------------------------------------
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("Eu sou o host.");
            // StartCoroutine(SpawnBazuca());
        }
    }

    //--------------------------------------------------------
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            Debug.Log("Acabei de virar host.");
            //StartCoroutine(SpawnBazuca());
        }
    }

    //--------------------------------------------------------
    public IEnumerator SpawnBazuca()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));

            Vector3 position = Random.insideUnitSphere * 25;
            position.y = 0.25f;

            PhotonNetwork.InstantiateRoomObject("BAZUKA", 
                position, 
                Quaternion.Euler(270, Random.Range(0, 360), 180));
        }
    }
}

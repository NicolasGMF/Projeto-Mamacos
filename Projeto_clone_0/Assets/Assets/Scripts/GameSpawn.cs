using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameSpawn : MonoBehaviour
{
    public float gametime = 0;
    public int quantion = 0;
    public int spawnid = 0;
    public int ordaspawn = 0;
    public bool started = false;
    public PhotonView spawnpv;
    // Start is called before the first frame update
    void Start()
    {
        spawnpv = GetComponent<PhotonView>();
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            if (started)
            {
                gametime += 1 * Time.deltaTime;
                if(spawnid==1)
                {
                    if (gametime > 10 && ordaspawn == 0 && quantion == 0)
                    {
                        var maca = PhotonNetwork.Instantiate("MacaAnimal", transform.position, transform.rotation);
                        maca.GetComponent<MacaAnimal>().spawner = this.gameObject;
                        maca.GetComponent<MacaAnimal>().lado = "direita";
                        quantion++;
                        //ordaspawn = 1; se comentado fica infinito
                    }
                }
                if(spawnid==2)
                {
                    if (gametime > 15 && ordaspawn == 0 && quantion == 0)
                    {
                        Debug.Log(quantion);
                        var maca = PhotonNetwork.Instantiate("MacaAnimal", transform.position, transform.rotation);
                        maca.GetComponent<MacaAnimal>().spawner = this.gameObject;
                        maca.GetComponent<MacaAnimal>().lado = "esquerda";
                        //maca.GetComponent<MacaAnimal>().GetComponent<PhotonView>().RPC("Setspa", RpcTarget.OthersBuffered, this.gameObject); ;
                        quantion++;
                        //ordaspawn = 1;
                    }
                }
            }
        }
    }
}

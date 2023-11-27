using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class AirdropDrop : MonoBehaviour
{

    public bool button1;

    public float coldCompra = 15;
    public bool airdropBuy = false;
    public PhotonView pv;



    [PunRPC]
    void AirdropBuying()
    {
        airdropBuy = true;
    }



    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {


        if (coldCompra > 0) coldCompra -= 60 * Time.deltaTime;
        button1 = Input.GetKey("1");

        if (PhotonNetwork.LocalPlayer.IsMasterClient) //debuggers
        {
            var button1Debbug = Input.GetKey("[1]");
            var button2Debbug = Input.GetKey("[2]");
            var button3Debbug = Input.GetKey("[3]");
            if (button1Debbug) GlobalVariables.Instance.money += 10;
            if (button2Debbug) GlobalVariables.Instance.timer += 0.1f;
            if (button3Debbug)
            {
                var amogus = PhotonNetwork.Instantiate("Amogus", new Vector3(Random.Range(-5, 5), 30, Random.Range(-5, 5)), Quaternion.Euler(Vector3.up * Random.Range(0, 360.0f)));
                var randsca = Random.Range(0.2f,5);
                amogus.transform.localScale = new Vector3(randsca, randsca, randsca);
            }

        }
        if (button1)
        {
            if(coldCompra<0)
            {
                if(GlobalVariables.Instance.money>=50)
                {
                    coldCompra = 15;
                    if (PhotonNetwork.LocalPlayer.IsMasterClient)
                    {
                        airdropBuy = true;
                    }
                    else
                    {
                        pv.RPC("AirdropBuying", RpcTarget.OthersBuffered);
                    }
                }
            }
        }

        
        if(airdropBuy)
        {
            GlobalVariables.Instance.money -= 50;
            Debug.Log("Airdrop caindo");
            Vector3 position = new Vector3(Random.Range(-5,5), 30, Random.Range(-5, 5));
            Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360.0f));
            PhotonNetwork.Instantiate("Airdrop", position, rotation);
            airdropBuy = false;
        }



    }
}

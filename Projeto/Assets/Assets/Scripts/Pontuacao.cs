using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityEngine.Android;

public class Pontuacao : MonoBehaviour
{

    public TMP_Text moneyText;
    public TMP_Text timerText;
    public TMP_Text fimText;
    public TMP_Text startText;
    public AudioSource music;
    public PhotonView pv;

    public bool apagado = false;

    [PunRPC]
    void SetTempo(float times, int moneys)
    {
        GlobalVariables.Instance.timer = times;
        GlobalVariables.Instance.money = moneys;
        //spawner = spawners;
    }

    [PunRPC]
    void Pause(bool pause)
    {
        GlobalVariables.Instance.paused = pause;
    }
    [PunRPC]
    void Comeca()
    {
        GlobalVariables.Instance.comeco = true ;
    }



    // Start is called before the first frame update
    void Start()
    {
        GlobalVariables.Instance.timer = 180;
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {

        if (GlobalVariables.Instance.comeco)
        {
            if(!apagado)
            {
                startText.gameObject.SetActive(false);
                apagado = true;
            }
            if (!GlobalVariables.Instance.paused)
            {
                moneyText.text = "$: " + GlobalVariables.Instance.money.ToString();
                timerText.text = "Timer: " + (((int)(GlobalVariables.Instance.timer / 60))).ToString("00") + ":" + ((int)(GlobalVariables.Instance.timer % 60)).ToString("00");
                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                {
                    GlobalVariables.Instance.timer -= Time.deltaTime;
                    pv.RPC("SetTempo", RpcTarget.OthersBuffered, GlobalVariables.Instance.timer, GlobalVariables.Instance.money);
                }
            }
            else
            {
                music.volume = 0.5f;
                fimText.gameObject.SetActive(true);
                fimText.text = "Fim de jogo.<br>Sua pontuacao: " + GlobalVariables.Instance.money.ToString();
            }
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                if (GlobalVariables.Instance.timer <= 0)
                {
                    GlobalVariables.Instance.paused = true;
                    pv.RPC("Pause", RpcTarget.OthersBuffered, true);
                }
            }

        }
        else
        {
            if (PhotonNetwork.LocalPlayer.IsMasterClient)
            {
                startText.text = "APERTE --->ESPACO<----\r\nQUANDO TODOS OS JOGADORES \r\nESTIVEREM PRONTOS";
                var buttonStart = Input.GetKey("space");
                if(buttonStart)
                {
                    GlobalVariables.Instance.comeco = true;
                    pv.RPC("Comeca", RpcTarget.OthersBuffered);
                }
            }
            else
            {
                startText.text = "Aguarde o Host Iniciar";
            }
        }
    }





}

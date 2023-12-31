using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEditor;

public class GameConnection : MonoBehaviourPunCallbacks
{
    public string roomName = "PUCC";
    public string playerNickname = "";

    public PlayerController mySelf;
    public Animator animator;

    void Start()
    {
        //conecta no Photon usando o arquivos de configuracoes
        Debug.Log("Conectando no servidor...");
        PhotonNetwork.NickName = playerNickname;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        //quando estamos conectados no servidor
        //entra num lobby para listar a salas do jogo
        base.OnConnectedToMaster();
        Debug.Log("Conectado no servidor!");
        PhotonNetwork.JoinLobby();
        Debug.Log("Entrando no lobby...");
    }

    public override void OnJoinedLobby()
    {
        //dentro do lobby podemos listar as salas
        //ou criar uma nova
        base.OnJoinedLobby();
        Debug.Log("Entrou no lobby!");

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;

        Debug.Log("Entrando na sala : " + roomName);
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, null);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("Player entrou na sala: " + newPlayer.NickName);
        mySelf.SendMyColor();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("Player saiu na sala: " + otherPlayer.NickName);
    }

    public override void OnJoinedRoom()
    {

        var playernumber = PhotonNetwork.LocalPlayer.ActorNumber;
        Debug.Log("Eu sou o jogador: " + playernumber);
        


        //aqui VOCE entrou numa sala
        Debug.Log("Entrei na sala: " + PhotonNetwork.CurrentRoom.Name);
        //base.OnJoinedRoom();
        Vector3 position = new Vector3(0, 4, 0);
        Quaternion rotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360.0f));
        if(playernumber==1)
        {
            GlobalVariables.Instance.myPlayer = PhotonNetwork.Instantiate("Player", position, rotation);
        }
        if(playernumber==2)
        {
            GlobalVariables.Instance.myPlayer = PhotonNetwork.Instantiate("Player2", position, rotation);
        }
        if (playernumber == 3)
        {
            GlobalVariables.Instance.myPlayer = PhotonNetwork.Instantiate("Player3", position, rotation);
        }
        if (playernumber == 4)
        {
            GlobalVariables.Instance.myPlayer = PhotonNetwork.Instantiate("Player4", position, rotation);
        }
        mySelf = GlobalVariables.Instance.myPlayer.GetComponent<PlayerController>();
        animator = mySelf.GetComponentInChildren<Animator>();
    }
}

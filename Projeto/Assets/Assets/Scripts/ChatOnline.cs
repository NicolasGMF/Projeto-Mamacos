using UnityEngine;
using UnityEngine.UI;

public class ChatOnline : MonoBehaviour
{
    public GameConnection myConnection;
    public Text chatText;
    void Start()
    {
        chatText.text = "";
    }

    public void AddChatText(string text, string nickname)
    {
        chatText.text += "[" + nickname + "] " + text + "\n";
    }


    public void SendChatMessage(string message)
    {
        if (myConnection != null && myConnection.mySelf != null)
        {
            myConnection.mySelf.SendChatMessage(message);
        }
    }

}

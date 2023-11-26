using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Pontuacao : MonoBehaviour
{

    public TMP_Text moneyText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = "$: " + GlobalVariables.Instance.money.ToString();
    }
}

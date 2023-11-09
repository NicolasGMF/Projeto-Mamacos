using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pausemenu : MonoBehaviour
{
    private string cenas = "Menu";
    public static bool Parandojogo = false;
    public GameObject pausemenuUi;
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(Parandojogo)
            {
                Continuar();
            }else
            {
                pause();
            }
        }

        
    }
      public void Continuar()
    {
        pausemenuUi.SetActive(false);
        Time.timeScale = 1.0f;
        Parandojogo = false;
    }
    void pause()
    {
        pausemenuUi.SetActive(true);
        Time.timeScale = 1f;
        Parandojogo=true;
    }
    public void carregarmenu()
    {
        Time.timeScale= 1.0f;
        SceneManager.LoadScene(cenas);
    }
    public void sairdojogo() 
    {
        Application.Quit();
        Debug.Log("Morri");
    }
}

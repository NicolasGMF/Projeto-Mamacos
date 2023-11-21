using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelloader : MonoBehaviour
{
    public Animator transition;
    public float transitiontempo= 3f;
    void Update()
    {
       /*  if (Input.GetButtonDown("Game"))
        {
            Carregarproximacena();
        }*/
    }
    public void loadscene()
    {
        SceneManager.LoadScene("Game");
    }

    public void loadmenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Carregarproximacena()
    {
        /* StartCoroutine(Carregacena(SceneManager.GetActiveScene().buildIndex + 1));*/

    }
    /*IEnumerator Carregacena(int levelIndex)
  {
      transition.SetTrigger("Start");
      yield return new WaitForSeconds(levelIndex);
      SceneManager.LoadScene(levelIndex);
  }*/
}

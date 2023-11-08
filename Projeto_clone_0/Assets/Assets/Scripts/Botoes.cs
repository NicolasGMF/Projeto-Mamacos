using UnityEngine;
using UnityEngine.SceneManagement;

public class Botoes : MonoBehaviour
{
    private string cena = "Game";
    public void CarregarJogo()
    {
        SceneManager.LoadScene(cena);
    }
    public void SairDoJogo()
    {
        Application.Quit();
        Debug.Log("Morri");
    }
}
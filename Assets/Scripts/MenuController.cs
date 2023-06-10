
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuController : MonoBehaviour
{
    public void Sair()
    {
        Application.Quit();
    }
     public void PlayGame()
    {
        SceneManager.LoadScene("fase01");
    }
}

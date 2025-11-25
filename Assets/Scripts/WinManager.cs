using UnityEngine;
using UnityEngine.SceneManagement;


public class WinManager : MonoBehaviour
{
    public void PlayAgain()
    {
        Debug.Log("Cargando juego de nuevo...");
        SceneManager.LoadScene(0); // Escena principal
    }
   
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
       
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

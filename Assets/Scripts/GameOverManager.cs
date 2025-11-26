using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    public void TryAgain()
    {
        // Borrar TODOS los PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        
        Debug.Log("Cargando juego de nuevo...");
        SceneManager.LoadScene(1); // Escena principal
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

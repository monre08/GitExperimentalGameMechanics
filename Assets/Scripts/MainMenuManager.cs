using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Llamado por el botón Play
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // Llamado por el botón Exit
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();

    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}

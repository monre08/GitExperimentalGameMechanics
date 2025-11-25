using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ExitDetector : MonoBehaviour
{
    [Header("Configuración")]
    public float winDelay = 1f;
    
    private bool hasWon = false;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Jugador ha tocado: {other.gameObject.name}");
        
        if (other.CompareTag("Yard") && !hasWon)
        {
            hasWon = true;
            WinGame();
        }
    }
    
    void WinGame()
    {
        // Mostrar mensaje
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowCollectionMessage("You escaped!");
        }

        // Eliminar Inventario Guardado
        PlayerPrefs.DeleteKey("Inventory");
        PlayerPrefs.Save();
        Debug.Log("Inventario eliminado tras Win");
        
        // Cargar escena de victoria
        StartCoroutine(LoadWinSceneAfterDelay(winDelay));
    }
    
    IEnumerator LoadWinSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(2); // WinScene (índice 2)
    }
}
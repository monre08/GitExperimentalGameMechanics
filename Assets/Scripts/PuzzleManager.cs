using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PuzzleManager : MonoBehaviour
{
    [Header("Configuración")]
    public int puzzleNumber = 1; // 1 o 2
    public float returnDelay = 3f;
    
    public void PuzzleCompleted()
    {
        Debug.Log($"Puzzle {puzzleNumber} completado!");

        // GUARDAR INVENTARIO antes de cambiar de escena
        InventoryManager inv = FindFirstObjectByType<InventoryManager>();
        if (inv != null) inv.SaveInventory();

        // Esperar antes de volver al laberinto
        StartCoroutine(ReturnToMainGameAfterDelay());
    }

    IEnumerator ReturnToMainGameAfterDelay()
    {
        yield return new WaitForSeconds(returnDelay);
        
        // Guardar progreso
        if (puzzleNumber == 1)
        {
            PlayerPrefs.SetInt("Puzzle1Completed", 1);
        }
        else if (puzzleNumber == 2)
        {
            PlayerPrefs.SetInt("Puzzle2Completed", 1);
        }
        PlayerPrefs.Save();

        DoorController.OnPuzzleCompleted(puzzleNumber);
        
        // Volver a la escena principal
        SceneManager.LoadScene(1);
    }
    
}
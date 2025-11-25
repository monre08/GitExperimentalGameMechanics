using UnityEngine;
using UnityEngine.SceneManagement;


public class DoorController : MonoBehaviour
{
    [Header("Configuración de Puerta")]
    public int doorNumber = 1; // 1 o 2
   
    private bool isCompleted = false;
    private SpriteRenderer spriteRenderer;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        EmotionalStateManager.Instance.OnStateChanged += OnEmotionalStateChanged;
        UpdateDoorState();
    }


    void OnDestroy()
    {
        if (EmotionalStateManager.Instance != null)
            EmotionalStateManager.Instance.OnStateChanged -= OnEmotionalStateChanged;
    }


    void OnEmotionalStateChanged(EmotionalState newState)
    {
        Debug.Log($"Puerta {doorNumber} detectó cambio de estado: {newState}");
        UpdateDoorState();
    }


    void UpdateDoorState()
    {
        EmotionalState currentState = EmotionalStateManager.Instance.GetCurrentState();

        // PUERTA 1: Solo disponible en Depressed
        if (doorNumber == 1)
        {
            if (currentState == EmotionalState.Depressed)
            {
                UnlockDoor();
            }
            else
            {
                LockDoor();
            }
        }
        // PUERTA 2: Solo disponible en Sad  
        else if (doorNumber == 2)
        {
            if (currentState == EmotionalState.Sad)
            {
                UnlockDoor();
            }
            else
            {
                LockDoor();
            }
        }
    }

    void UnlockDoor()
    {
        isCompleted = false;
        GetComponent<Collider2D>().enabled = true;
    }

    void LockDoor()
    {
        isCompleted = true;
        GetComponent<Collider2D>().enabled = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCompleted)
        {
            EmotionalState currentState = EmotionalStateManager.Instance.GetCurrentState();
           
            // Verificar estado correcto para cada puerta
            if ((doorNumber == 1 && currentState == EmotionalState.Depressed) ||
                (doorNumber == 2 && currentState == EmotionalState.Sad))
            {
                Debug.Log($"Puerta {doorNumber} tocada - Cargando minijuego");
                SavePlayerPosition();

                // Guardar inventario
                InventoryManager inventory = other.GetComponent<InventoryManager>();
                if (inventory != null) 
                {
                    inventory.SaveInventory();
                }
                
                // Cargar puzzle
                SceneManager.LoadScene(doorNumber == 1 ? "PuzzleScene1" : "PuzzleScene2");
            }
        }
    }


    void SavePlayerPosition()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (doorNumber == 1)
            {
                PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x + 1f);
            } else {
                PlayerPrefs.SetFloat("PlayerPosX", player.transform.position.x - 1f);
            }

            PlayerPrefs.SetFloat("PlayerPosY", player.transform.position.y);
            PlayerPrefs.Save();
            Debug.Log("Posición guardada para puerta " + doorNumber);
        }
    }


    // Llamar a este método cuando se completa un puzzle
    public static void OnPuzzleCompleted(int puzzleNumber)
    {
        if (puzzleNumber == 1)
        {
            EmotionalStateManager.Instance.AdvanceState(); // Depressed → Sad
        }
        else if (puzzleNumber == 2)
        {
            EmotionalStateManager.Instance.AdvanceState(); // Sad → Hope
        }
        PlayerPrefs.Save();
       
        Debug.Log($"Puzzle {puzzleNumber} completado - Estado avanzado!");
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public enum EmotionalState
{
    Depressed,
    Sad,
    Hope
}


public class EmotionalStateManager : MonoBehaviour
{
    public static EmotionalStateManager Instance;
   
    [SerializeField] private EmotionalState currentState = EmotionalState.Depressed;
   
    // Eventos para cuando cambia el estado
    public System.Action<EmotionalState> OnStateChanged;
   
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
       
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Escena cargada: {scene.name}");
       
        if (scene.buildIndex == 0 && ShouldReset()) // Escena principal
        {
            ResetToDepressed();
        }
    }


    // Determinar cuándo resetear
    bool ShouldReset()
    {
        // Si NO venimos de un puzzle, resetear
        return !PlayerPrefs.HasKey("PlayerPosX") && !PlayerPrefs.HasKey("PlayerPosY");
    }
   
    // Resetear a Depressed
    public void ResetToDepressed()
    {
        currentState = EmotionalState.Depressed;
        OnStateChanged?.Invoke(currentState);

        // // RESETEAR COMPLETO DE PUERTAS
        // PlayerPrefs.DeleteKey("Puzzle1Completed");
        // PlayerPrefs.DeleteKey("Puzzle2Completed");
        // PlayerPrefs.DeleteKey("PlayerPosX");
        // PlayerPrefs.DeleteKey("PlayerPosY");
        // PlayerPrefs.Save();

        Debug.Log("Estado emocional reseteado a: Depressed");
    }
   
    public EmotionalState GetCurrentState()
    {
        return currentState;
    }
   
    public void ChangeState(EmotionalState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(newState);
            Debug.Log($"Estado emocional cambiado a: {newState}");
        }
    }
   
    public void AdvanceState()
    {
        if (currentState < EmotionalState.Hope)
        {
            EmotionalState newState = currentState + 1;
            ChangeState(newState);
            Debug.Log($"Estado avanzado de {currentState-1} a {newState}");
        }
    }
   
    public void RegressState()
    {
        if (currentState > EmotionalState.Depressed)
        {
            ChangeState(currentState - 1);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

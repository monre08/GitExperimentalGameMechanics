using UnityEngine;
using System.Collections;

public class WatchTimer : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    public float countdownTime = 30f; // 30 segundos de cuenta atrás
    
    [Header("Referencias")]
    public TimerManager timerManager;
    
    private bool hasBeenActivated = false;
    
    void Start()
    {
        // Buscar TimerManager si no está asignado
        if (timerManager == null)
            timerManager = FindFirstObjectByType<TimerManager>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenActivated)
        {
            Debug.Log("¡Reloj activado! Iniciando cuenta atrás...");
            hasBeenActivated = true;
            
            if (timerManager != null)
            {
                // Iniciar la cuenta atrás de 30 segundos
                timerManager.StartCountdown(countdownTime);
                
                // Mostrar mensaje
                UIManager uiManager = FindFirstObjectByType<UIManager>();
                if (uiManager != null)
                {
                    uiManager.ShowCollectionMessage("Countdown started! 30s");
                }
            }
            else
            {
                Debug.LogError("TimerManager no encontrado!");
            }
            
            // Opcional: Hacer invisible el reloj pero mantener el collider
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
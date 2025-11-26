using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


public class TimerManager : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    public float currentTime;
    public bool isTimerRunning = false;
   
    [Header("UI References")]
    public TextMeshProUGUI timerText;
    public GameObject timerPanel;


    [Header("Game Over")]
   
    // Evento cuando el tiempo se acaba
    public System.Action OnTimeOut;
   
    void Start()
    {
        // NO iniciar el temporizador automáticamente
        currentTime = 0f;
        UpdateTimerDisplay();
       
        // Ocultar el panel al inicio
        if (timerPanel != null) timerPanel.SetActive(false);
    }
   
    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
           
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                TimerEnded();
            }
        }
    }
   
    // Iniciar cuenta atrás
    public void StartCountdown(float seconds)
    {
        currentTime = seconds;
        isTimerRunning = true;
       
        if (timerPanel != null) timerPanel.SetActive(true);
        Debug.Log($"Cuenta atrás iniciada: {seconds} segundos");
    }
   
    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Temporizador detenido!");
    }


    void TimerEnded()
    {
        StopTimer();
        OnTimeOut?.Invoke();
       
        // Mostrar mensaje de tiempo agotado
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowCollectionMessage("Time's up!");
        }
       
        // Mostrar escena de Game Over tras 2 segundos
        StartCoroutine(LoadGameOverAfterDelay(2f));
    }


    IEnumerator LoadGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        // Cargar por índice del Build Settings
        SceneManager.LoadScene(2);
    }
   
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
           
            // Cambiar color cuando queda poco tiempo
            if (currentTime < 10f)
            {
                timerText.color = Color.red;
            }
            else if (currentTime < 30f)
            {
                timerText.color = Color.yellow;
            }
            else
            {
                timerText.color = Color.white;
            }
        }
    }
   
    public bool IsTimerRunning()
    {
        return isTimerRunning;
    }
}

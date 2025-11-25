using UnityEngine;
using TMPro;

public class LightOrbUsage : MonoBehaviour
{
    [Header("Configuración")]
    public float timeToAdd = 15f; // 15 segundos extra por orbe
   
    [Header("UI References")]
    public GameObject usagePanel;
    public TextMeshProUGUI usageText;
   
    private InventoryManager inventory;
    private TimerManager timerManager;
    private bool canUseOrb = false;
   
    void Start()
    {
        inventory = FindFirstObjectByType<InventoryManager>();
        timerManager = FindFirstObjectByType<TimerManager>();
       
        if (usagePanel != null)
        {
            usagePanel.SetActive(false);
        }
       
        // Suscribirse a eventos del timer
        if (timerManager != null)
        {
            timerManager.OnTimeOut += OnTimerEnded;
        }
    }
   
    void OnDestroy()
    {
        if (timerManager != null)
        {
            timerManager.OnTimeOut -= OnTimerEnded;
        }
    }
   
    void Update()
    {
        // Verificar si se puede usar un orbe (cuando algún timer está activo)
        canUseOrb = (timerManager != null && timerManager.IsTimerRunning() &&
                    inventory != null && inventory.GetItemCount(CollectableType.LightOrb) > 0);
       
        // Mostrar/ocultar panel de uso
        if (usagePanel != null)
        {
            usagePanel.SetActive(canUseOrb);
        }
       
        // Tecla para usar orbe,'O'
        if (canUseOrb && Input.GetKeyDown(KeyCode.O))
        {
            UseLightOrb();
        }
    }
   
    void UseLightOrb()
    {
        if (inventory.UseItem(CollectableType.LightOrb))
        {
            // Añadir tiempo al timer activo
            timerManager.currentTime += timeToAdd;
           
            // Mostrar mensaje
            UIManager uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                uiManager.ShowCollectionMessage($"+{timeToAdd} seconds added!");
            }
           
            Debug.Log($"Orbe de luz usado! +{timeToAdd} segundos");
        }
    }
   
    void OnTimerEnded()
    {
        canUseOrb = false;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Mensajes de Recolección")]
    public GameObject collectionMessagePanel;
    public TextMeshProUGUI collectionText;
    public float messageDisplayTime = 10f;

    [Header("Panel de Inventario")]
    public GameObject inventoryPanel;
    public TextMeshProUGUI heartsText;
    public TextMeshProUGUI orbsText;
    public TextMeshProUGUI slotsText;

    private InventoryManager inventory;
    private EmotionalStateManager emotionalState;

    void Start()
    {

        inventory = FindFirstObjectByType<InventoryManager>();
        if (inventory == null)
        {
            Debug.LogError("No se encontró InventoryManager en la escena!");
            // Buscar en el Player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                inventory = player.GetComponent<InventoryManager>();
                if (inventory == null)
                    Debug.Log("InventoryManager no encontrado en el Player");
            }
        }


        // Suscribirse al evento de cambios en el inventario
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateInventoryUI;
        }

        // Buscar componentes si no están asignados
        if (collectionMessagePanel == null)
            collectionMessagePanel = GameObject.Find("CollectionMessagePanel");
        if (inventoryPanel == null)
            inventoryPanel = GameObject.Find("InventoryPanel");

        // Ocultar paneles
        if (collectionMessagePanel != null) 
        {
            collectionMessagePanel.SetActive(false);
        }
        if (inventoryPanel != null) 
        {
            inventoryPanel.SetActive(false);
        }

    }

    void Update()
    {
        // Tecla para mostrar/ocultar inventario (I)
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ShowCollectionMessage(string message)
    {
        if (collectionMessagePanel != null && collectionText != null)
        {
            collectionMessagePanel.SetActive(true);
            collectionText.text = message;
            StartCoroutine(HideMessageAfterTime());
        }
    }

    private IEnumerator HideMessageAfterTime()
    {
        yield return new WaitForSeconds(messageDisplayTime);
        collectionMessagePanel.SetActive(false);
    }

    void UpdateInventoryUI(InventoryManager inv)
    {
        if (inventoryPanel != null && inventoryPanel.activeInHierarchy)
        {
            UpdateInventoryDisplay();
        }
    }

    void UpdateInventoryDisplay()
    {
        if (inventory == null) return;

        heartsText.text = $"Hearts: {inventory.GetItemCount(CollectableType.Heart)}";
        orbsText.text = $"Light Orbs: {inventory.GetItemCount(CollectableType.LightOrb)}";
        slotsText.text = $"Slots: {inventory.GetItemCount(CollectableType.Heart) + inventory.GetItemCount(CollectableType.LightOrb)}/4";
    }

    void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            bool isActive = !inventoryPanel.activeInHierarchy;
            inventoryPanel.SetActive(isActive);
            
            if (isActive)
            {
                UpdateInventoryDisplay();
            }
        }
    }

    public void ShowLightOrbPrompt(bool show)
    {
        if (collectionMessagePanel != null)
        {
            collectionMessagePanel.SetActive(show);
            if (show && collectionText != null)
            {
                collectionText.text = "Press O to use Light Orb for extra time!";
            }
        }
    }

    void OnDestroy()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged -= UpdateInventoryUI;
        }
    }
}
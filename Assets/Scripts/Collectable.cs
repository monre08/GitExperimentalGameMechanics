using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum CollectableType
{
    Heart,
    LightOrb
}

public class Collectable : MonoBehaviour
{
    public CollectableType type;
    
    Light2D orbLight;
    public float apagarDelay = 1f;

    void Start()
    {
        // Busca la luz automáticamente en los componentes hijos
        orbLight = GetComponentInChildren<Light2D>();
    }

   
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Trigger detectado con: {other.gameObject.name}");
       
        if (other.CompareTag("Player"))
        {            
            InventoryManager inventory = other.GetComponent<InventoryManager>();
            if (inventory != null)
            {
                if (inventory.AddItem(type))
                {
                    Debug.Log($"¡{type} recolectado exitosamente!");

                    ShowCollectionMessage(type);
                    if (orbLight != null) StartCoroutine(ApagaYDestruye());
                    else Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventario lleno o error al añadir");
                    ShowInventoryFullMessage();
                }
            }
            else
            {
                Debug.LogError("No se encontró inventario en el jugador");
            }
        }
    }

    System.Collections.IEnumerator ApagaYDestruye()
    {
        // Apaga la luz visualmente
        orbLight.intensity = 0;
        yield return new WaitForSeconds(apagarDelay);
        Destroy(gameObject);
    }


    // Mensajes para los coleccionables
    void ShowCollectionMessage(CollectableType itemType)
    {
        string message = itemType switch
        {
            CollectableType.Heart => "Heart collected! \nPress I to check inventory",
            CollectableType.LightOrb => "Light Orb collected! \nPress I to check inventory",
            _ => "Item collected!"
        };
       
        Debug.Log(message);
       
        // Buscar UIManager en la escena
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowCollectionMessage(message);
        }
        else
        {
            Debug.LogError("UIManager NO encontrado en la escena!");
        }
    }


    // Mensaje de Inventario lleno
    void ShowInventoryFullMessage()
    {
        string message = "No space left!";
       
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowCollectionMessage(message);
        }
    }

}

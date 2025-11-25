using UnityEngine;
using System.Collections;


public class WallDamageController : MonoBehaviour
{
    private InventoryManager inventoryManager;
    private bool canTakeDamage = true;
    // 5 segundos para quitarse de la pared antes de volver a perder otra vida
    private float damageCooldown = 5f;
   
    void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager no encontrado en el Player!");
        }
       
        // Mostrar advertencia inicial
        ShowInitialWarning();
    }


    // Métodos para mostrar advertencia inicial
    void ShowInitialWarning()
    {
        StartCoroutine(ShowWarningAfterDelay(1f)); // Esperar 1 segundo al inicio
    }
    IEnumerator ShowWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
       
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowCollectionMessage("Don't touch the walls!");
        }
    }
   
    // Ha tocado la pared
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && canTakeDamage)
        {
            Debug.Log("Colisión con pared detectada");
           
            if (inventoryManager != null)
            {
                inventoryManager.LoseHeartFromWall();
                StartCoroutine(DamageCooldown());
            }
        }
    }
   
    // Segundos de inmunidad tras tocar la pared
    IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}
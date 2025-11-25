using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System; 
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private int maxSlots = 4;
    private List<CollectableType> inventory = new List<CollectableType>();
   
    public System.Action<InventoryManager> OnInventoryChanged;
    public System.Action OnGameOver;
   
    void Start()
    {
        // Para comprobar si tenemos inventario o es partida nueva
        LoadInventory();
    }

    public bool AddItem(CollectableType item)
    {
        if (inventory.Count < maxSlots)
        {
            inventory.Add(item);
            OnInventoryChanged?.Invoke(this);
            Debug.Log($"Añadido: {item}. Inventario: {inventory.Count}/{maxSlots}");
            return true;
        }
        else
        {
            Debug.Log("Inventario lleno!");
            return false;
        }
    }

    // Bajar 1 vida del inventario cuando toca la pared
    public void LoseHeartFromWall()
    {
        if (GetItemCount(CollectableType.Heart) > 0)
        {
            // Eliminar un corazón del inventario
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i] == CollectableType.Heart)
                {
                    inventory.RemoveAt(i);
                    OnInventoryChanged?.Invoke(this);
                   
                    Debug.Log($"Vida perdida. Corazones restantes: {GetItemCount(CollectableType.Heart)}");
                   
                    // Mostrar mensaje en el juego
                    UIManager uiManager = FindFirstObjectByType<UIManager>();
                    if (uiManager != null)
                    {
                        uiManager.ShowCollectionMessage("You touched the wall! \nYou lost a heart");
                    }
                   
                    // Verificar Game Over
                    if (GetItemCount(CollectableType.Heart) <= 0)
                    {
                        StartCoroutine(GameOverSequence());
                    }
                   
                    break;
                }
            }
        }
    }
   
    public bool UseItem(CollectableType item)
    {
        if (inventory.Contains(item))
        {
            inventory.Remove(item);
            OnInventoryChanged?.Invoke(this);
            return true;
        }
        return false;
    }
   
    public int GetItemCount(CollectableType type)
    {
        int count = 0;
        foreach (var item in inventory)
        {
            if (item == type) count++;
        }
        return count;
    }
   
    public bool IsFull()
    {
        return inventory.Count >= maxSlots;
    }

    // Si ya no le quedan vidas en el inventario Game Over
    private IEnumerator GameOverSequence()
    {
        UIManager uiManager = FindFirstObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowCollectionMessage("You ran out of lives!");
        }
       
        Debug.Log("Game Over - Sin corazones en el inventario");

        // Eliminar Inventario Guardado
        PlayerPrefs.DeleteKey("Inventory");
        PlayerPrefs.Save();
        Debug.Log("Inventario eliminado tras Game Over");
       
        yield return new WaitForSeconds(2f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void SaveInventory()
    {
        // Convierte el inventario en una cadena tipo: "Heart,LightOrb,Heart"
        string invStr = string.Join(",", inventory.Select(item => item.ToString()).ToArray());
        PlayerPrefs.SetString("Inventory", invStr);
        PlayerPrefs.Save();
        Debug.Log("Inventario guardado: " + invStr);
    }

    public void LoadInventory()
    {
        inventory.Clear();
        if (PlayerPrefs.HasKey("Inventory"))
        {
            string invStr = PlayerPrefs.GetString("Inventory");
            string[] items = invStr.Split(',');
            foreach (var itemStr in items)
            {
                if (Enum.TryParse(itemStr, out CollectableType result))
                {
                    inventory.Add(result);
                }
            }
            Debug.Log("Inventario cargado: " + invStr);
            OnInventoryChanged?.Invoke(this);
        }
        else
        {
            // Si no hay nada guardado, inicia con un corazón
            AddItem(CollectableType.Heart);
            Debug.Log("Inventario vacío, iniciando con 1 corazón");
        }
    }

}
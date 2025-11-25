using UnityEngine;


public class PlayerPositionLoader : MonoBehaviour
{
    void Start()
    {
        Invoke("LoadPlayerPosition", 0.1f); // Con delay
    }
   
    void LoadPlayerPosition()
    {
        Debug.Log("Buscando posición guardada del player...");


        // Solo cargar posición si venimos de un minijuego
        if (PlayerPrefs.HasKey("PlayerPosX") && PlayerPrefs.HasKey("PlayerPosY"))
        {
            float posX = PlayerPrefs.GetFloat("PlayerPosX");
            float posY = PlayerPrefs.GetFloat("PlayerPosY");
           
            transform.position = new Vector2(posX, posY);


            Debug.Log($"Posición CARGADA: ({posX}, {posY})");
           
            Invoke("ClearSavedPosition", 0.5f);
        }
        else
        {
            // Posición inicial por defecto
            transform.position = new Vector2(-0.65f, -10f);
            Debug.Log("Player en posición inicial por defecto");
        }
    }


   
    void ClearSavedPosition()
    {
        PlayerPrefs.DeleteKey("PlayerPosX");
        PlayerPrefs.DeleteKey("PlayerPosY");
        PlayerPrefs.Save();
        Debug.Log("Posición guardada limpiada");
    }
}

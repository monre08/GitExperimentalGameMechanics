using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnvironmentController : MonoBehaviour
{
    [Header("Referencias")]
    public Light2D globalLight;
    public ParticleSystem rainParticles;
    
    [Header("Configuración por Estado")]
    public Color depressedColor = Color.gray;
    public Color sadColor = Color.blue;
    public Color hopeColor = Color.green;
    
    void Start()
    {
        // Suscribirse a los cambios de estado
        if (EmotionalStateManager.Instance != null)
        {
            EmotionalStateManager.Instance.OnStateChanged += UpdateEnvironment;
            
            // Configuración inicial
            UpdateEnvironment(EmotionalStateManager.Instance.GetCurrentState());
        }
        else
        {
            Debug.LogError("EmotionalStateManager no encontrado!");
        }
    }

    void OnDestroy()
    {
        // Desuscribirse para evitar errores
        if (EmotionalStateManager.Instance != null)
        {
            EmotionalStateManager.Instance.OnStateChanged -= UpdateEnvironment;
        }
    }
    
    void UpdateEnvironment(EmotionalState state)
    {
        switch (state)
        {
            case EmotionalState.Depressed:
                globalLight.color = depressedColor;
                globalLight.intensity = 0.3f;
                if (rainParticles != null) rainParticles.Play();
                break;
                
            case EmotionalState.Sad:
                globalLight.color = sadColor;
                globalLight.intensity = 0.6f;
                if (rainParticles != null) rainParticles.Stop();
                break;
                
            case EmotionalState.Hope:
                globalLight.color = hopeColor;
                globalLight.intensity = 0.8f;
                if (rainParticles != null) rainParticles.Stop();
                break;
        }
    }
}
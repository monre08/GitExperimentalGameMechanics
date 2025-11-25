using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FieldOfViewController : MonoBehaviour
{
    [Header("Configuración de Visión")]
    public float depressedRadius = 2f;
    public float sadRadius = 8f;
    public float hopeRadius = 16f;
    
    [Header("Referencias")]
    public Light2D visionLight; // Una luz circular que simula la visión
    
    void Start()
    {
        // Suscribirse a cambios de estado emocional
        EmotionalStateManager.Instance.OnStateChanged += UpdateVision;
        UpdateVision(EmotionalStateManager.Instance.GetCurrentState());
    }
    
    void OnDestroy()
    {
        if (EmotionalStateManager.Instance != null)
            EmotionalStateManager.Instance.OnStateChanged -= UpdateVision;
    }
    
    void UpdateVision(EmotionalState state)
    {
        if (visionLight == null) return;
        
        switch (state)
        {
            case EmotionalState.Depressed:
                visionLight.pointLightOuterRadius = depressedRadius;
                break;
            case EmotionalState.Sad:
                visionLight.pointLightOuterRadius = sadRadius;
                break;
            case EmotionalState.Hope:
                visionLight.pointLightOuterRadius = hopeRadius;
                break;
        }
        
        Debug.Log($"Visión actualizada a radio: {visionLight.pointLightOuterRadius}");
    }
}
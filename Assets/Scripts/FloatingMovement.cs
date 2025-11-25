using UnityEngine;

public class FloatingMovement : MonoBehaviour
{
    [Header("Movimiento Aleatorio")]
    public float moveRadius = 3f; // Radio máximo de movimiento
    public float moveSpeed = 1.5f;  // Velocidad de movimiento
    
    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float timeToChangeTarget;
    
    void Start()
    {
        startPosition = transform.position;
        SetNewTargetPosition();
    }
    
    void Update()
    {
        // Mover hacia la posición objetivo
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        
        // Si llegamos al objetivo o pasó el tiempo, cambiar objetivo
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f || Time.time >= timeToChangeTarget)
        {
            SetNewTargetPosition();
        }
    }
    
    void SetNewTargetPosition()
    {
        // Posición aleatoria dentro del radio
        Vector2 randomOffset = Random.insideUnitCircle * moveRadius;
        targetPosition = startPosition + randomOffset;
        
        // Tiempo aleatorio para cambiar de dirección (2-4 segundos)
        timeToChangeTarget = Time.time + Random.Range(2f, 4f);
    }

}
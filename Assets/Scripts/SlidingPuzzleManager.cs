using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SlidingPuzzleManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;

    [Header("Configuración del Puzzle")]
    public int gridRows = 4;
    public int gridColums = 3;
    public Sprite completeImage;
    public List<Sprite> puzzlePieces;

    [Header("Tamaño de Celdas")]
    public float cellSize = 150f; 
    public float spacing = 5f;
    
    [Header("Referencias UI")]
    public GameObject puzzleGrid;
    public GameObject puzzlePiecePrefab;

    [Header("Timer Configuration")]
    public bool hasTimer = false;
    public float timeLimit = 30f; // segundos para el puzzle 2
    
    private List<GameObject> puzzlePiecesList = new List<GameObject>();
    private GameObject emptySlot;
    private Vector2 emptySlotPosition;
    private int moveCount = 0;
    private GridLayoutGroup gridLayout;
    private TimerManager timerManager;
    private bool puzzleCompleted = false;
    
    void Start()
    {
        gridLayout = puzzleGrid.GetComponent<GridLayoutGroup>();
        timerManager = FindFirstObjectByType<TimerManager>();

        InitializePuzzle();
        ShufflePuzzle();
            
        if (victoryPanel != null) 
        {
            victoryPanel.SetActive(false);
        }

        // Configurar timer solo si este puzzle tiene límite de tiempo
        if (hasTimer && timerManager != null)
        {
            timerManager.OnTimeOut += OnPuzzleTimeOut;
            timerManager.StartCountdown(timeLimit);

            // MOSTRAR mensaje del orbe si hay orbes y un timer
            InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
            UIManager uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager != null && inventory != null && inventory.GetItemCount(CollectableType.LightOrb) > 0)
            {
                uiManager.ShowCollectionMessage("Press O to use Light Orb for extra time!");
            }

        }
    }
    
    void OnDestroy()
    {
        if (timerManager != null)
        {
            timerManager.OnTimeOut -= OnPuzzleTimeOut;
        }
    }

    void InitializePuzzle()
    {        
        // Crear todas las piezas CON Grid Layout Group ACTIVO
        for (int i = 0; i < puzzlePieces.Count; i++)
        {
            GameObject piece = Instantiate(puzzlePiecePrefab, puzzleGrid.transform);
            piece.name = $"Piece_{i}";
            
            Image image = piece.GetComponent<Image>();
            if (image != null && i < puzzlePieces.Count)
            {
                image.sprite = puzzlePieces[i];
            }
            
            Button button = piece.GetComponent<Button>();
            if (button != null)
            {
                int index = i; // Capturar el índice para el evento
                button.onClick.AddListener(() => OnPieceClicked(piece));
            }
            
            puzzlePiecesList.Add(piece);
        }
        
        // Configurar el espacio vacío (pieza 2)
        int emptySlotIndex = 2;
        emptySlot = puzzlePiecesList[emptySlotIndex];
        emptySlotPosition = emptySlot.GetComponent<RectTransform>().anchoredPosition;
        emptySlot.SetActive(false);
        
        // Desactivar Grid Layout Group y posicionar manualmente
        if (gridLayout != null)
        {
            gridLayout.enabled = false;
        }
        
        // Posicionar manualmente todas las piezas
        PositionPiecesManually();
    }

    void OnPuzzleTimeOut()
    {
        if (!puzzleCompleted)
        {
            
            // Cargar escena de Game Over
            StartCoroutine(LoadGameOverAfterDelay(2f));
        }
    }

    IEnumerator LoadGameOverAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(2); // GameOverScene
    }
    
    void PositionPiecesManually()
    {
        float startX = -((gridColums - 1) * (cellSize + spacing)) / 2f;
        float startY = ((gridRows - 1) * (cellSize + spacing)) / 2f;
        
        int pieceIndex = 0;
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColums; col++)
            {
                if (pieceIndex < puzzlePiecesList.Count)
                {
                    GameObject piece = puzzlePiecesList[pieceIndex];
                    RectTransform rect = piece.GetComponent<RectTransform>();
                    
                    if (rect != null)
                    {
                        Vector2 position = new Vector2(
                            startX + col * (cellSize + spacing),
                            startY - row * (cellSize + spacing)
                        );
                        
                        rect.anchoredPosition = position;
                        rect.sizeDelta = new Vector2(cellSize, cellSize);
                        
                        // Guardar posición original para esta pieza
                        if (piece == emptySlot)
                        {
                            emptySlotPosition = position;
                        }
                    }
                    pieceIndex++;
                }
            }
        }
    }
    
    void OnPieceClicked(GameObject clickedPiece)
    {
        if (clickedPiece == emptySlot) return;
        
        Vector2 clickedPosition = clickedPiece.GetComponent<RectTransform>().anchoredPosition;
        
        Debug.Log($"Pieza clickeada: {clickedPiece.name}");
        Debug.Log($"Posición: {clickedPosition}");
        Debug.Log($"Espacio vacío en: {emptySlotPosition}");
        
        if (IsAdjacentToEmptySlot(clickedPosition))
        {
            Debug.Log("Movimiento válido");
            MovePiece(clickedPiece, clickedPosition);
            moveCount++;
            
            if (IsPuzzleSolved())
            {
                PuzzleCompleted();
            }
        }
        else
        {
            Debug.Log("No adyacente al espacio vacío");
        }
    }
    
    bool IsAdjacentToEmptySlot(Vector2 piecePosition)
    {
        float totalSize = cellSize + spacing;
        float distance = Vector2.Distance(piecePosition, emptySlotPosition);
        
        // Está adyacente si la distancia es aproximadamente el tamaño de una celda
        bool isAdjacent = Mathf.Abs(distance - totalSize) < 5f;
        
        Debug.Log($"Distancia: {distance}, Tamaño celda: {totalSize}, Adyacente: {isAdjacent}");
        return isAdjacent;
    }
    
    void MovePiece(GameObject piece, Vector2 piecePosition)
    {
        // Guardar posición temporal
        Vector2 oldEmptyPosition = emptySlotPosition;
        
        // Mover la pieza visualmente al espacio vacío
        piece.GetComponent<RectTransform>().anchoredPosition = emptySlotPosition;
        
        // Actualizar posición del espacio vacío
        emptySlotPosition = piecePosition;
        
        Debug.Log($" {piece.name} movido a {oldEmptyPosition}");
    }
    
    void ShufflePuzzle()
    {
        Debug.Log("Mezclando puzzle...");
        
        // Hacer varios movimientos aleatorios válidos
        int shuffleMoves = 50;
        for (int i = 0; i < shuffleMoves; i++)
        {
            List<GameObject> adjacentPieces = GetAdjacentPieces();
            if (adjacentPieces.Count > 0)
            {
                GameObject randomPiece = adjacentPieces[Random.Range(0, adjacentPieces.Count)];
                Vector2 piecePosition = randomPiece.GetComponent<RectTransform>().anchoredPosition;
                MovePiece(randomPiece, piecePosition);
            }
        }
        
        moveCount = 0;
        Debug.Log("Puzzle mezclado");
    }
    
    List<GameObject> GetAdjacentPieces()
    {
        List<GameObject> adjacentPieces = new List<GameObject>();
        
        foreach (GameObject piece in puzzlePiecesList)
        {
            if (piece == emptySlot || !piece.activeInHierarchy) continue;
            
            Vector2 piecePosition = piece.GetComponent<RectTransform>().anchoredPosition;
            if (IsAdjacentToEmptySlot(piecePosition))
            {
                adjacentPieces.Add(piece);
            }
        }
        
        return adjacentPieces;
    }
    
    bool IsPuzzleSolved()
    {
        // Verificar si todas las piezas están en su posición original
        int pieceIndex = 0;
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColums; col++)
            {
                if (pieceIndex < puzzlePiecesList.Count)
                {
                    GameObject piece = puzzlePiecesList[pieceIndex];
                    Vector2 expectedPosition = GetExpectedPosition(row, col);
                    Vector2 actualPosition = piece.GetComponent<RectTransform>().anchoredPosition;
                    
                    if (piece == emptySlot)
                    {
                        // Para emptySlotIndex = 2, debería estar en [0,2]
                        if (row != 0 || col != 2)
                            return false;
                    }
                    else if (Vector2.Distance(actualPosition, expectedPosition) > 5f)
                    {
                        return false;
                    }
                    pieceIndex++;
                }
            }
        }

        ShowVictoryMessage();

        Debug.Log("¡PUZZLE RESUELTO!");
        return true;
    }

    void ShowVictoryMessage()
    {
        // Mostrar panel de victoria
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        
        // Configurar texto
        if (victoryText != null)
        {
            victoryText.text = $"PUZZLE {FindFirstObjectByType <PuzzleManager>().puzzleNumber} COMPLETED!";
        }
    }
    
    Vector2 GetExpectedPosition(int row, int col)
    {
        float startX = -((gridColums - 1) * (cellSize + spacing)) / 2f;
        float startY = ((gridRows - 1) * (cellSize + spacing)) / 2f;
        
        return new Vector2(
            startX + col * (cellSize + spacing),
            startY - row * (cellSize + spacing)
        );
    }
    
    void PuzzleCompleted()
    {
        puzzleCompleted = true;
        Debug.Log($"Puzzle completado en {moveCount} movimientos!");

        // Detener el timer si estaba corriendo
        if (hasTimer && timerManager != null)
        {
            timerManager.StopTimer();
        }

        FindFirstObjectByType <PuzzleManager>().PuzzleCompleted();
    }
    
    void ReturnToMainGame()
    {
        FindFirstObjectByType <PuzzleManager>().PuzzleCompleted();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public RectTransform parentRect;
    private readonly float aspectRatio = 1200f / 200f; // 6:1

    void Start()
    {
        UpdateGrid();
    }

    void Update()
    {
        UpdateGrid();
    }

    void UpdateGrid()
    {
        if (gridLayout == null || parentRect == null) return;

        float parentWidth = parentRect.rect.width;
        int columns = gridLayout.constraintCount; // Number of columns

        float spacing = gridLayout.spacing.x * (columns - 1); // Total spacing between cells
        float cellWidth = (parentWidth - spacing) / columns;
        float cellHeight = cellWidth / aspectRatio; // Maintain original 6:1 ratio

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}



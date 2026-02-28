using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridCell _cellPrefab;
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 2f;
    [SerializeField] private Material normalTileMaterial;
    [SerializeField] private Material pathTileMaterial;

    public GridCell[,] Grid { get; private set; }

    private void Awake()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        Grid = new GridCell[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 position = new Vector3(x * _cellSize, 0, y * _cellSize);

                GridCell cell = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
                cell.Initialize(new Vector2Int(x, y));

                // Mark middle row as path
                if (y == 5)
                {
                    cell.SetAsPathCell();
                }

                Grid[x, y] = cell;
            }
        }
    }
}
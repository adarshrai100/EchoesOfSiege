using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int _width = 10;
    [SerializeField] private int _height = 10;
    [SerializeField] private float _cellSize = 2f;
    [SerializeField] private GridCell _cellPrefab;

    private GridCell[,] _grid;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _grid = new GridCell[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Vector3 position = new Vector3(
                    x * _cellSize,
                    0,
                    z * _cellSize
                );

                GridCell cell = Instantiate(_cellPrefab, position, Quaternion.identity, transform);
                cell.Initialize(new Vector2Int(x, z));

                _grid[x, z] = cell;
            }
        }
    }
}
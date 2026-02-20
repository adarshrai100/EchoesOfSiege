using UnityEngine;
using UnityEngine.InputSystem;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _towerPrefab;

    private GridCell _currentCell;

    private void Update()
    {
        HandleMouseHover();
        HandlePlacement();
    }

    private void HandleMouseHover()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();

            if (cell != null)
            {
                if (_currentCell != cell)
                {
                    ClearCurrentCell();
                    _currentCell = cell;
                    _currentCell.Highlight(true);
                }
            }
            else
            {
                ClearCurrentCell();
            }
        }
        else
        {
            ClearCurrentCell();
        }
    }

    private void ClearCurrentCell()
    {
        if (_currentCell != null)
        {
            _currentCell.Highlight(false);
            _currentCell = null;
        }
    }

    private void HandlePlacement()
    {
        if (_currentCell == null) return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!_currentCell.IsOccupied)
            {
                PlaceTower(_currentCell);
            }
        }
    }

    private void PlaceTower(GridCell cell)
    {
        Vector3 position = cell.transform.position;
        position.y += 1f; // raise tower slightly above grid

        Instantiate(_towerPrefab, position, Quaternion.identity);

        cell.SetOccupied(true);
    }
}
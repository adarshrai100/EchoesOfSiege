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
        GameObject tower = Instantiate(_towerPrefab);

        float towerHalfHeight = tower.transform.localScale.y / 2f;

        Vector3 position = cell.transform.position;
        position.y += (cell.transform.localScale.y / 2f) + towerHalfHeight;

        tower.transform.position = position;

        cell.SetOccupied(true);
    }

    private void ClearCurrentCell()
    {
        if (_currentCell != null)
        {
            _currentCell.Highlight(false);
            _currentCell = null;
        }
    }
}
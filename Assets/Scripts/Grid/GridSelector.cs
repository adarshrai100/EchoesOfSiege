using UnityEngine;
using UnityEngine.InputSystem;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private int _towerCost = 25;
    [SerializeField] private ObjectPool _projectilePool;
    [SerializeField] private LayerMask _towerLayer;

    private GridCell _currentCell;

    private void Update()
    {
        HandleMouseHover();
        HandleTowerSelection();
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
            if (!_currentCell.IsOccupied && _resourceManager.CanAfford(_towerCost))
            {
                PlaceTower(_currentCell);
                _resourceManager.Spend(_towerCost);
            }
        }
    }

    private void PlaceTower(GridCell cell)
    {
        GameObject tower = Instantiate(_towerPrefab);

        // Get grid top surface from collider bounds
        Collider gridCollider = cell.GetComponent<Collider>();
        float gridTop = gridCollider.bounds.max.y;

        // Get tower half height from its collider bounds
        Collider towerCollider = tower.GetComponent<Collider>();
        float towerHalfHeight = towerCollider.bounds.extents.y;

        Vector3 position = cell.transform.position;
        position.y = gridTop + towerHalfHeight;

        tower.transform.position = position;

        TowerBase towerBase = tower.GetComponent<TowerBase>();
        towerBase.Initialize(_projectilePool);

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

    private void HandleTowerSelection()
    {
        if (TowerSelectionUI.Instance != null && TowerSelectionUI.Instance.IsPanelOpen)
            return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = _camera.ScreenPointToRay(mousePosition);

            // Try select tower
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _towerLayer))
            {
                TowerBase tower = hit.collider.GetComponent<TowerBase>();
                if (tower != null)
                {
                    TowerSelectionUI.Instance?.SelectTower(tower);
                    return;
                }
            }

            // If clicked but not on tower → deselect
            TowerSelectionUI.Instance?.DeselectTower();
        }
    }
}
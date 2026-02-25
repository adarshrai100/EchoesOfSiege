using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject _towerPrefab;
    [SerializeField] private ObjectPool _projectilePool;
    [SerializeField] private ResourceManager _resourceManager;
    [SerializeField] private int _towerCost = 50;

    private GridCell _currentCell;

    private void Update()
    {
        HandleMouseHover();
        HandleTowerSelection();
        HandlePlacement();
    }

    private void HandleMouseHover()
    {
        if (TowerSelectionUI.Instance != null && TowerSelectionUI.Instance.IsPanelOpen)
        {
            ClearCurrentCell();
            return;
        }

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
                return;
            }
        }

        ClearCurrentCell();
    }

    private void HandleTowerSelection()
    {
        if (!Mouse.current.leftButton.wasPressedThisFrame)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _camera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            TowerBase tower = hit.collider.GetComponent<TowerBase>();

            if (tower != null)
            {
                TowerSelectionUI.Instance?.SelectTower(tower);
                return;
            }
        }

        TowerSelectionUI.Instance?.DeselectTower();
    }

    private void HandlePlacement()
    {
        if (TowerSelectionUI.Instance != null && TowerSelectionUI.Instance.IsPanelOpen)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

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
        Renderer renderer = tower.GetComponentInChildren<Renderer>();
        float yOffset = renderer.bounds.extents.y;

        tower.transform.position = cell.transform.position + Vector3.up * yOffset;

        TowerBase towerBase = tower.GetComponent<TowerBase>();
        towerBase.Initialize(_projectilePool, cell);
        towerBase.RegisterInitialCost(_towerCost);

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
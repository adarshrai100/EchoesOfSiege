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
    private GameObject _ghostTower;

    private void Update()
    {
        HandleMouseHover();
        HandleTowerSelection();
        HandlePlacement();
    }

    // =========================
    // HOVER LOGIC
    // =========================
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
                }

                bool canPlace = !_currentCell.IsOccupied &&
                                !_currentCell.IsPathCell &&
                                _resourceManager.CanAfford(_towerCost);

                if (canPlace)
                    _currentCell.SetVisualState(GridCell.CellVisualState.Valid);
                else
                    _currentCell.SetVisualState(GridCell.CellVisualState.Invalid);

                UpdateGhostTower(cell, canPlace);

                return;
            }
        }

        ClearCurrentCell();
    }

    // =========================
    // TOWER SELECTION LOGIC
    // =========================
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

    // =========================
    // PLACEMENT LOGIC
    // =========================
    private void HandlePlacement()
    {
        if (TowerSelectionUI.Instance != null && TowerSelectionUI.Instance.IsPanelOpen)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (_currentCell == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (!_currentCell.IsOccupied &&
                !_currentCell.IsPathCell &&
                _resourceManager.CanAfford(_towerCost))
            {
                PlaceTower(_currentCell);
                _resourceManager.Spend(_towerCost);
            }
        }
    }

    // =========================
    // PLACE TOWER
    // =========================
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

    // =========================
    // GHOST TOWER SYSTEM
    // =========================
    private void UpdateGhostTower(GridCell cell, bool valid)
    {
        if (_ghostTower == null)
        {
            _ghostTower = Instantiate(_towerPrefab);

            TowerBase towerBase = _ghostTower.GetComponent<TowerBase>();
            if (towerBase != null)
                towerBase.enabled = false;

            Collider col = _ghostTower.GetComponentInChildren<Collider>();
            if (col != null)
                col.enabled = false;
        }

        Renderer renderer = _ghostTower.GetComponentInChildren<Renderer>();
        float yOffset = renderer.bounds.extents.y;

        _ghostTower.transform.position = cell.transform.position + Vector3.up * yOffset;

        Color color = valid ? new Color(0.3f, 1f, 0.3f, 0.5f) : new Color(1f, 0.3f, 0.3f, 0.5f);
        renderer.material.color = color;
    }

    // =========================
    // CLEAR CELL
    // =========================
    private void ClearCurrentCell()
    {
        if (_currentCell != null)
        {
            _currentCell.SetVisualState(GridCell.CellVisualState.Normal);
            _currentCell = null;
        }

        if (_ghostTower != null)
        {
            Destroy(_ghostTower);
            _ghostTower = null;
        }
    }
}
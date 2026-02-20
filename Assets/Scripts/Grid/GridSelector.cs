using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    private GridCell _currentCell;

    private void Update()
    {
        HandleMouseHover();
    }

    private void HandleMouseHover()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

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
}
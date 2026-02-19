using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private Transform[] _waypoints;

    public int WaypointCount => _waypoints.Length;

    public Transform GetWaypoint(int index)
    {
        if (index < 0 || index >= _waypoints.Length)
        {
            Debug.LogError("Waypoint index out of range.");
            return null;
        }

        return _waypoints[index];
    }

    public Transform GetStartWaypoint()
    {
        return _waypoints[0];
    }

    private void OnDrawGizmos()
    {
        if (_waypoints == null || _waypoints.Length == 0) return;

        Gizmos.color = Color.green;

        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            if (_waypoints[i] != null && _waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
            }
        }
    }

}

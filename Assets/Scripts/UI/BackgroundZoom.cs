using UnityEngine;

public class BackgroundZoom : MonoBehaviour
{
    [SerializeField] private float zoomAmount = 0.03f;
    [SerializeField] private float zoomSpeed = 0.05f;

    private Vector3 initialScale;

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * zoomSpeed) * zoomAmount;

        transform.localScale = initialScale * scale;
    }
}
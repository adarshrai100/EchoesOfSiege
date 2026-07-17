using UnityEngine;

public class LogoFloat : MonoBehaviour
{
    [SerializeField] private float floatAmount = 6f;
    [SerializeField] private float speed = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * floatAmount;
        transform.localPosition = startPosition + Vector3.up * offset;
    }
}
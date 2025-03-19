using UnityEngine;

public class Floaty : MonoBehaviour
{
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatHeight = 1f;

    Vector3 floatDelta;
    Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        floatDelta = Vector3.up * (floatHeight * Mathf.Sin(floatSpeed * Time.time));
        transform.position = startPosition + floatDelta;
    }
}
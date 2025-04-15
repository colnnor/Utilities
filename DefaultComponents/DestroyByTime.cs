using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float time = 1f;

    private void Start()
    {
        Destroy(gameObject, time);
        
    }

    public void SetTime(float time)
    {
        this.time = time;
    }
    public void SetTimeRange(float min, float max)
    {
        time = Random.Range(min, max);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
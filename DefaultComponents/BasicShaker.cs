using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class BasicShaker : MonoBehaviour
{
    [SerializeField] private bool infiniteShake = false;
    [HideIf("infiniteShake")]
    [SerializeField] private float shakeDuration = 0.5f;
    [HideIf("infiniteShake")]
    [SerializeField] private AnimationCurve shakeFalloff = AnimationCurve.EaseInOut(0, 1, 1, 0);
    
    [SerializeField] private float shakeMagnitude = 0.1f;
    [SerializeField] private float shakeFrequency = 20f;
    
    private Vector3 originalPosition;
    private float shakeTime;
    private bool isShaking;
    
    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (isShaking)
        {
            Shake();
        }
    }
    
    [Button("Start Shake")]
    public void StartShake()
    {
        isShaking = true;
        shakeTime = 0f;
    }
    
    public void StopShake()
    {
        isShaking = false;
        transform.localPosition = originalPosition;
    }
    
    private void Shake()
    {
        if(infiniteShake)
        {
            Vector3 offset = Random.insideUnitSphere * shakeMagnitude;
            transform.localPosition = originalPosition + offset;
        }
        else if(shakeTime < shakeDuration)
        {
            shakeTime += Time.deltaTime;
            float t = shakeTime / shakeDuration;
            float falloff = shakeFalloff.Evaluate(t);
            Vector3 offset = Random.insideUnitSphere * (shakeMagnitude * falloff);
            transform.localPosition = originalPosition + offset;
        }
        else
        {
            StopShake();
        }
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlickeringLight : MonoBehaviour
{
    [SerializeField] private Light lightSource;
    [SerializeField] private float flickerFadeDuration = 0.1f;
    [SerializeField] private Vector2 flickerRange = new (0.5f, 1.5f);
    private float flickerValue;

    private void Reset()
    {
        lightSource = gameObject.GetComponentOrInChildren<Light>();
    }

    private void Start()
    {
        lightSource ??= gameObject.GetComponentOrInChildren<Light>();
        if (lightSource)
        {
            NewIntensity();
            return;
        }

        Debug.Log($"No light source found on {gameObject.name}.", this);
        enabled = false;
    }

    void NewIntensity()
    {
        flickerValue = flickerRange.RandomBetween();
        StartCoroutine(FadeToNewIntensity());
    }

    IEnumerator FadeToNewIntensity()
    {
        float t = 0f;
        float intensity = lightSource.intensity;

        while (t < flickerFadeDuration)
        {
            t += Time.deltaTime;
            lightSource.intensity = Mathf.Lerp(intensity, flickerValue, t / flickerFadeDuration);
            yield return null;
        }

        lightSource.intensity = flickerValue;
        NewIntensity();
    }
}
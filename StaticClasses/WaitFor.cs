using System.Collections.Generic;
using UnityEngine;

public class WaitFor : MonoBehaviour
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new();
    
    public static WaitForSeconds Seconds(float seconds)
    {
        if (WaitDictionary.TryGetValue(seconds, out WaitForSeconds wait)) return wait;

        WaitDictionary[seconds] = new WaitForSeconds(seconds);
        return WaitDictionary[seconds];
        
    }
}

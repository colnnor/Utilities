
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList", menuName = "Utilities/SceneList", order = 1)]
public class SceneList : ScriptableObject
{
    public List<string> favoriteScenes = new List<string>();
    [SerializeField] public Dictionary<string, DateTime> lastOpenedScenes = new Dictionary<string, DateTime>();
    public List<string> allScenes = new List<string>();
}
using Sirenix.OdinInspector;
using UnityEngine;

public class SetupLOD : MonoBehaviour
{public int numberOfLODLevels = 3; // Number of LOD levels
    public float[] screenRelativeTransitionHeights = { 0.6f, 0.4f, 0.2f }; // Transition heights for each LOD level

    [Button("Setup")]
    void Setup()
    {
        // Ensure there is a Renderer component
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError("No Renderer found on the GameObject.");
            return;
        }

        // Add LODGroup component if not already present
        LODGroup lodGroup = gameObject.GetComponent<LODGroup>();
        if (lodGroup == null)
        {
            lodGroup = gameObject.AddComponent<LODGroup>();
        }

        // Create an array for LOD levels
        LOD[] lods = new LOD[numberOfLODLevels];

        // Set the Renderer for each LOD level
        for (int i = 0; i < numberOfLODLevels; i++)
        {
            lods[i] = new LOD(screenRelativeTransitionHeights[i], new Renderer[] { renderer });
        }

        // Assign the LODs to the LODGroup
        lodGroup.SetLODs(lods);
        

        // Recalculate the LOD bounds
        lodGroup.RecalculateBounds();
    }
}
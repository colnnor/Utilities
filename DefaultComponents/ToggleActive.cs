using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    public GameObject target;
    public void Toggle()
    {
        if (target)
        {
            target.SetActive(!target.activeSelf);
        }
    }
}

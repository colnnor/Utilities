using UnityEngine;

public class DestroyOthersSingleton : MonoBehaviour
{
    [Tooltip("Those with lower priority will be destroyed. If two have the same priority, the one created last will be kept.")]
    [SerializeField] private int priority = 0;
    public static DestroyOthersSingleton Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }
        if(Instance == this) return;
        
        if (Instance.priority >= priority)
        {
            Destroy(gameObject);
            return;
        }

        Destroy(Instance.gameObject);
        Instance = this;
    }
}
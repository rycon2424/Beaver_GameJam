using Sirenix.OdinInspector;
using UnityEngine;

public class ResourceAble : MonoBehaviour, IInteractable
{
    [Header("Resource")]
    public int yieldAmount;
    public ItemTypes resourceType;
    public int health = 1;
    [ReadOnly] public bool yielded;

    public virtual void OnInteract(Transform playerTransform)
    {
        if (yielded)
            return;
        
        health--;
        if (health == 0)
        {
            yielded = true;
            UIManager.Singleton.UpdateItem(resourceType, yieldAmount);
        }
    }
}
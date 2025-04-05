using Sirenix.OdinInspector;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody rb;
    int health;

    [Button]
    public void OnInteract()
    {
        health--;
        if(health <= 0)
        {
            rb.isKinematic = false;
            //drop tree
        }
    }
}

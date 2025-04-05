using Sirenix.OdinInspector;
using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Vector3 force;
    [SerializeField] int health = 1;

    [Button]
    public void OnInteract()
    {
        health--;
        if(health <= 0)
        {
            rb.isKinematic = false;
            rb.AddForce(force);
            //drop tree
        }
    }
}

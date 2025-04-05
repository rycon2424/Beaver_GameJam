using UnityEngine;

public class Tree : MonoBehaviour, IInteractable
{
    int health;

    public void OnInteract()
    {
        health--;
        if(health <= 0)
        {
            //drop tree
        }
    }
}

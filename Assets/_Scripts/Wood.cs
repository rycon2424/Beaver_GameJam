using UnityEngine;

public class Wood : ResourceAble, IInteractable
{
    public override void OnInteract(Transform playerTransform)
    {
        base.OnInteract(playerTransform);

        // Do visuals
        Destroy(gameObject);
    }
}

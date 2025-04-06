using System.Collections.Generic;
using UnityEngine;

public class WoodCollector : MonoBehaviour
{
    public Rigidbody playerRb;
    public List<Wood> woods = new List<Wood>();
    PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public Wood GetLastWood()
    {
        if (woods.Count == 0)
            return null;

        return woods[woods.Count - 1];
    }

    public int RemoveAllWood(bool destroy = false)
    {
        int count = 0;
        for (int i = woods.Count - 1; i >= 0; i--)
        {
            Destroy(woods[i].joint);

            woods[i].yielded = false;
            woods[i].health = 1;
            UIManager.Singleton.UpdateItem(ItemTypes.Wood, -1);

            if (destroy)
                Destroy(woods[i].gameObject);

            woods.RemoveAt(i);
            count++;
        }

        playerController.UpdateMovementSpeed(woods.Count);
        return count;
    }

    public void RemoveWood(Wood wood)
    {
        if (woods.Count == 0)
            return;

        int brokenWood = int.MaxValue;
        for (int i = 0; i < woods.Count; i++)
        {
            if (woods[i] == wood)
            {
                brokenWood = i;
                break;
            }
        }

        for (int i = woods.Count - 1; i >= 0; i--)
        {
            if (i >= brokenWood)
            {
                Destroy(woods[i].joint);

                woods[i].yielded = false;
                woods[i].health = 1;

                woods.RemoveAt(i);

                UIManager.Singleton.UpdateItem(ItemTypes.Wood, -1);
            }
        }

        playerController.UpdateMovementSpeed(woods.Count);
    }

    public void AddWood(Wood wood)
    {
        woods.Add(wood);
        playerController.UpdateMovementSpeed(woods.Count);
    }
}

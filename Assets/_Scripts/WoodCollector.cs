using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WoodCollector : MonoBehaviour
{
    public Rigidbody playerRb;
    public List<Wood> woods = new List<Wood>();

    public Wood GetLastWood()
    {
        if (woods.Count == 0)
            return null;

        return woods[woods.Count - 1];
    }
    
    public void RemoveAllWood()
    {
        RemoveWood(woods[0]);
    }
   
    public void RemoveWood(Wood wood)
    {
        if (woods.Count == 0) return;

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
                woods.RemoveAt(i);
            }
        }
    }

    public void AddWood(Wood wood)
    {
        woods.Add(wood);
    }
}

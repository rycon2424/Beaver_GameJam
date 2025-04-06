using System;
using UnityEngine;

public class KidsManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (UIManager.Singleton.itemVisuals.ContainsKey(ItemTypes.Berries))
            {
                int currentBerries =  UIManager.Singleton.itemVisuals[ItemTypes.Berries].count;

                GameManager.Singleton.babiesCurrentFood += currentBerries;
            
                UIManager.Singleton.UpdateItem(ItemTypes.Berries, -currentBerries);
            }
        }
    }
}

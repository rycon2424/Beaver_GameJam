using UnityEngine;

public class WoodForDam : MonoBehaviour
{
    public WoodCollector wCollector;

    


    private bool playerPresent;
    //public string playerTag;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Debug.Log("yo");
            if(wCollector == null)
                wCollector = other.GetComponent<WoodCollector>();

            if (!playerPresent)
            {
                playerPresent = true;

                for (int i = wCollector.woods.Count - 1; i >= 0; i--)
                {
                    Destroy(wCollector.woods[i].gameObject);
                    wCollector.woods.RemoveAt(i);
                    UIManager.Singleton.UpdateItem(ItemTypes.Wood, -1);

                    GameManager.Singleton.damCurrentHealth += 1;
                    
                    DamManager.Singleton.ChangeHealth();
                }

                //wCollector.RemoveAllWood();

            }
                

        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerPresent = false;
    }
}

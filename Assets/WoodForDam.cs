using UnityEngine;

public class WoodForDam : MonoBehaviour
{
    public WoodCollector wCollector;

    private bool playerPresent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (wCollector == null)
                wCollector = other.GetComponent<WoodCollector>();

            if (!playerPresent)
            {
                playerPresent = true;

                GameManager.Singleton.damCurrentHealth += wCollector.RemoveAllWood(true);
                DamManager.Singleton.ChangeHealth();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerPresent = false;
    }
}

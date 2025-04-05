using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    // Publieke variabele om het aantal hout te bekijken in de Inspector
    public int currentWood = 0;
    public int currentFood = 0;

    public ResourceType resourceType;

    private void Awake()
    {
        // Check of er al een instantie bestaat
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Vernietig duplicaten
            return;
        }

        Instance = this;
        // Optioneel: zorg dat het object niet verdwijnt bij scene changes
        // DontDestroyOnLoad(gameObject);
    }

    public void AddResource(int amount, ResourceType resourceType)
    {
        if (resourceType == ResourceType.wood)
        {
            if (amount > 0)
            {
                currentWood += amount;
                Debug.Log("Hout toegevoegd. Totaal hout: " + currentWood);
            }
            else
            {
                Debug.LogWarning("Kan geen negatief of nul aantal hout toevoegen.");
            }
        }
        else if (resourceType == resourceType.food)
        {
            if (amount > 0)
            {
                currentFood += amount;
                Debug.Log("Eten toegevoegd. Totaal eten: " + currentWood);
            }
            else
            {
                Debug.LogWarning("Kan geen negatief of nul aantal eten toevoegen.");
            }
        }
    }

    public void RemoveResource(int amountLost, ResourceType resourceType)
    {
        if (resourceType == resourceType.wood)
        {
            if (amountLost > 0 && currentWood >= amountLost) // Controleer of je genoeg hout hebt
            {
                currentWood -= amountLost;
                Debug.Log("Hout verloren. Totaal hout: " + currentWood);
            }
            else
            {
                Debug.LogWarning("Je hebt niet genoeg hout om te verliezen.");
            }
        }
        else if (resourceType == resourceType.food)
        {
            if (amountLost > 0 && currentWood >= amountLost) // Controleer of je genoeg eten hebt
            {
                currentFood -= amountLost;
                Debug.Log("Eten verloren. Totaal eten: " + currentFood);
            }
            else
            {
                Debug.LogWarning("Je hebt niet genoeg eten om te verliezen.");
            }
        }
    }
}
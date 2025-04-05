using UnityEngine;

public class WoodManager : MonoBehaviour
{
    public static WoodManager Instance;

    // Publieke variabele om het aantal hout te bekijken in de Inspector
    public int currentWood = 0;

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

    // Functie om hout toe te voegen
    public void AddWood(int amount)
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
}

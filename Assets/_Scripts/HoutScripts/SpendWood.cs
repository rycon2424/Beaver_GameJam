using UnityEngine;

public class SpendWood : MonoBehaviour
{
    public int cost = 1;  // De kosten in hout (standaard 1 hout)

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Rechtermuisklik
        {
            // Muispositie omzetten naar een ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Raycast checkt wat we raken
            if (Physics.Raycast(ray, out hit))
            {
                // Check of het object waar we op klikken dit script heeft
                SpendWood clicked = hit.collider.GetComponent<SpendWood>();

                if (clicked != null && clicked == this)
                {
                    if (WoodManager.Instance != null)
                    {
                        // Controleer of je genoeg hout hebt
                        if (WoodManager.Instance.currentWood >= cost)
                        {
                            // Als je genoeg hout hebt, verlies het
                            WoodManager.Instance.LoseWood(cost);
                            Debug.Log("Hout uitgegeven. Nieuw totaal: " + WoodManager.Instance.currentWood);
                        }
                        else
                        {
                            // Als je niet genoeg hout hebt, geef een waarschuwing
                            Debug.LogWarning("Je hebt niet genoeg hout om dit uit te geven!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("WoodManager Singleton niet gevonden!");
                    }
                }
            }
        }
    }
}

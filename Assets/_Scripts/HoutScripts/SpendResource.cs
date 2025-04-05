using UnityEngine;

public class SpendResource : MonoBehaviour
{
    public int cost = 1;  // De kosten (standaard 1)

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
                SpendResource clicked = hit.collider.GetComponent<SpendResource>();

                if (clicked != null && clicked == this)
                {
                    if (ResourceManager.Instance != null)
                    {
                        // Controleer of je genoeg hout hebt
                        if (ResourceManager.Instance.currentWood >= cost)
                        {
                            // Als je genoeg hout hebt, verlies het
                            ResourceManager.Instance.LoseWood(cost);
                            Debug.Log("Hout uitgegeven. Nieuw totaal: " + ResourceManager.Instance.currentWood);
                        }
                        else
                        {
                            // Als je niet genoeg hout hebt, geef een waarschuwing
                            Debug.LogWarning("Je hebt niet genoeg hout om dit uit te geven!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("ResourceManager Singleton niet gevonden!");
                    }
                }
            }
        }
    }
}

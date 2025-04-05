using UnityEngine;

public class ClickForResource : MonoBehaviour
{
    public ResourceType resourceType;

    private bool isClicked = false; // Variabele om bij te houden of het object al geklikt is

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
                ClickForResource clicked = hit.collider.GetComponent<ClickForResource>();

                if (clicked != null && clicked == this && !isClicked) // Zorg ervoor dat het object nog niet geklikt is
                {
                    // Zorg dat we het hout toevoegen
                    if (ResourceManager.Instance != null)
                    {
                        ResourceManager.Instance.AddResource(1, resourceType);
                        Debug.Log("Hout toegevoegd! Totaal hout: " + ResourceManager.Instance.currentWood);
                    }

                    // Markeer dit object als geklikt en zet alleen het script inactief
                    isClicked = true;
                    this.enabled = false; // Zet alleen het script inactief zodat het niet verder reageert
                }
            }
        }
    }
}

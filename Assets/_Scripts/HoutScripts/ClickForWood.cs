using UnityEngine;

public class ClickForWood : MonoBehaviour
{
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
                ClickForWood clicked = hit.collider.GetComponent<ClickForWood>();

                if (clicked != null && clicked == this && !isClicked) // Zorg ervoor dat het object nog niet geklikt is
                {
                    // Zorg dat we het hout toevoegen
                    if (WoodManager.Instance != null)
                    {
                        WoodManager.Instance.AddWood(1);
                        Debug.Log("Hout toegevoegd! Totaal hout: " + WoodManager.Instance.currentWood);
                    }

                    // Markeer dit object als geklikt en zet alleen het script inactief
                    isClicked = true;
                    this.enabled = false; // Zet alleen het script inactief zodat het niet verder reageert
                }
            }
        }
    }
}

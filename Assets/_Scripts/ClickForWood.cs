using UnityEngine;

public class ClickForWood : MonoBehaviour
{
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

                if (clicked != null && clicked == this)
                {
                    if (WoodManager.Instance != null)
                    {
                        WoodManager.Instance.AddWood(1);
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

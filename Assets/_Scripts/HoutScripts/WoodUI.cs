using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    private TextMeshProUGUI resourceText;

    void Start()
    {
        resourceText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (ResourceManager.Instance != null)
        {
            // Zorg ervoor dat de "Food" tekst een nieuwe regel krijgt na de "Hout" tekst
            resourceText.text = "Hout: " + ResourceManager.Instance.currentWood + "\nFood: " + ResourceManager.Instance.currentFood;
        }
    }
}

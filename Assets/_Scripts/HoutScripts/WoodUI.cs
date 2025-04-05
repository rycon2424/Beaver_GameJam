using TMPro;
using UnityEngine;

public class WoodDisplay : MonoBehaviour
{
    private TextMeshProUGUI woodText;

    void Start()
    {
        woodText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (ResourceManager.Instance != null)
        {
            woodText.text = "Hout: " + ResourceManager.Instance.currentWood;
        }
    }
}

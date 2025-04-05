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
        if (WoodManager.Instance != null)
        {
            woodText.text = "Hout: " + WoodManager.Instance.currentWood;
        }
    }
}

using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemVisual : SerializedMonoBehaviour
{
    public Image icon;
    public TMP_Text amount;
    [Space]
    public int count;

    public void Setup(Sprite itemSprite, int itemAmount)
    {
        icon.sprite = itemSprite;
        count = itemAmount;
        
        UpdateUI();
    }

    public void UpdateCount(int addition)
    {
        count += addition;
        
        UpdateUI();
    }

    private void UpdateUI()
    {
        amount.text = count.ToString();
    }
}

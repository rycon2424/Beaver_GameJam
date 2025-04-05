using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class UIManager : SerializedMonoBehaviour
{
    [Header("Bars to spawn")]
    [SerializeField] private Dictionary<string, Color> bars = new Dictionary<string, Color>();
    
    [Header("UI References")]
    [SerializeField] private Transform barContent;
    [SerializeField] private GameObject uiBarPrefab;

    private Dictionary<string, UIBar> spawnedBars = new Dictionary<string, UIBar>();

    private void Start()
    {
        spawnedBars = new Dictionary<string, UIBar>();
        
        foreach (KeyValuePair<string, Color> bar in bars)
        {
            GameObject go = Instantiate(uiBarPrefab, barContent);

            UIBar uiBar = new UIBar(go, 1, bar.Value);
            
            spawnedBars.Add(bar.Key, uiBar);
        }
    }
}

[System.Serializable]
public class UIBar
{
    private GameObject bar;
    private Image imageFill;

    public UIBar(GameObject _bar, float _startFill, Color _fillColor)
    {
        bar = _bar;
        
        imageFill = bar.transform.GetChild(0).GetComponent<Image>();
        imageFill.fillAmount = _startFill;
        imageFill.color = _fillColor;
    }
}

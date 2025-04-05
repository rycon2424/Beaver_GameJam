using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Serialization;

public class UIManager : SerializedMonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Dictionary<ItemTypes, Sprite> itemSprites = new Dictionary<ItemTypes, Sprite>();
    
    [Header("Bars to spawn")]
    [SerializeField] private Dictionary<string, Color> bars = new Dictionary<string, Color>();
    
    [Header("UI References")]
    [SerializeField] private Transform barContent;
    [SerializeField] private GameObject uiBarPrefab;
    [SerializeField] private Transform itemContent;
    [SerializeField] private GameObject itemPrefab;
    
    [Header("Timer")]
    public TMP_Text timerText;
    private Coroutine timerCoroutine;

    [HideInInspector] public Dictionary<string, UIBar> spawnedBars = new Dictionary<string, UIBar>();
    private Dictionary<ItemTypes, ItemVisual> itemVisuals = new Dictionary<ItemTypes, ItemVisual>();

    public static UIManager Singleton;

    [Button]
    private void Debug_AddItem(ItemTypes item, int amount)
    {
        UpdateItem(item, amount);
    }
    
    private void Awake()
    {
        if (Singleton != null)
            Destroy(Singleton.gameObject);
        Singleton = this;
    }

    private void Start()
    {
        timerText.text = "";
        spawnedBars = new Dictionary<string, UIBar>();
        
        foreach (KeyValuePair<string, Color> bar in bars)
        {
            GameObject go = Instantiate(uiBarPrefab, barContent);

            UIBar uiBar = new UIBar(go, 1, bar.Value);
            
            spawnedBars.Add(bar.Key, uiBar);
        }
    }
    
    public void UpdateItem(ItemTypes item, int amount)
    {
        if (itemVisuals.ContainsKey(item) == false && amount > 0)
        {
            ItemVisual iv = Instantiate(itemPrefab, itemContent).GetComponent<ItemVisual>();
        
            iv.Setup(itemSprites[item], amount);
            itemVisuals.Add(item, iv);
        }
        else
        {
            if (itemVisuals.ContainsKey(item))
            { 
                itemVisuals[item].UpdateCount(amount);
                
                if (itemVisuals[item].count <= 0)
                {
                    GameObject visual = itemVisuals[item].gameObject;
                    itemVisuals.Remove(item);
                
                    Destroy(visual);
                }
            }
        }
    }
    
    public void StartTimer()
    {
        if (timerCoroutine != null)
            StopCoroutine(timerCoroutine);

        timerCoroutine = StartCoroutine(RunTimer());
    }

    private IEnumerator RunTimer()
    {
        int totalSeconds = 0;

        while (true)
        {
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timerText.text = $"{minutes:D2}:{seconds:D2}";

            yield return new WaitForSeconds(1f);
            totalSeconds++;
        }
    }

    public void StopTimer()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
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

    public void UpdateFill(float maxValue, float currentValue)
    {
        imageFill.fillAmount = currentValue / maxValue;
    }
}

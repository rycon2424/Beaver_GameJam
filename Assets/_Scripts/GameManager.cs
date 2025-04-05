using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [Header("Game Manager")]
    public float damMaxHealth;
    [ReadOnly] public float damCurrentHealth;

    public float babiesMaxFood;
    [ReadOnly] public float babiesCurrentFood;

    [Header("Decay Settings - Dam")]
    public float damBaseDecayRate = 0.1f;
    public float damDecayAcceleration = 0.01f;

    [Header("Decay Settings - Babies")]
    public float babiesBaseDecayRate = 0.15f;
    public float babiesDecayAcceleration = 0.015f;

    private float damCurrentDecayRate;
    private float babiesCurrentDecayRate;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton.gameObject);
        }

        Singleton = this;
    }

    [Button]
    private void StartGame()
    {
        damCurrentHealth = damMaxHealth;
        babiesCurrentFood = babiesMaxFood;

        damCurrentDecayRate = damBaseDecayRate;
        babiesCurrentDecayRate = babiesBaseDecayRate;

        UIManager.Singleton.StartTimer();
        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            float delta = Time.deltaTime;

            // Decay both values independently
            damCurrentHealth = Mathf.Max(0f, damCurrentHealth - damCurrentDecayRate * delta);
            babiesCurrentFood = Mathf.Max(0f, babiesCurrentFood - babiesCurrentDecayRate * delta);

            // Increase each decay rate over time
            damCurrentDecayRate += damDecayAcceleration * delta;
            babiesCurrentDecayRate += babiesDecayAcceleration * delta;

            UIManager.Singleton.spawnedBars["Kids Needs"].UpdateFill(babiesMaxFood, babiesCurrentFood);
            UIManager.Singleton.spawnedBars["Dam Health"].UpdateFill(damMaxHealth, damCurrentHealth);
            
            yield return null;
        }
    }
}

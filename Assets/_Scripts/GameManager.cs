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

    [Header("Respawn Values")]
    public float berriesTimer;
    public float treeTimer;
    [Space]
    public GameObject treePrefab;

    [Header("Decay Settings - Dam")]
    public float damBaseDecayRate = 0.1f;
    public float damDecayAcceleration = 0.01f;

    [Header("Decay Settings - Babies")]
    public float babiesBaseDecayRate = 0.15f;
    public float babiesDecayAcceleration = 0.015f;

    private float damCurrentDecayRate;
    private float babiesCurrentDecayRate;

    [Header("References")]
    public GameObject[] tutorials;
    [Space]
    public Transform water;
    public float minY = -0.4f;
    public float maxY = 0.7f;
    
    [Header("GameOver")]
    public bool gameOver;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton.gameObject);
        }

        Singleton = this;
    }

    [Button]
    public void StartGame()
    {
        damCurrentHealth = damMaxHealth;
        babiesCurrentFood = babiesMaxFood;

        damCurrentDecayRate = damBaseDecayRate;
        babiesCurrentDecayRate = babiesBaseDecayRate;

        UIManager.Singleton.StartTimer();
        StartCoroutine(HideTutorials());
        StartCoroutine(GameLoop());
    }

    private IEnumerator HideTutorials()
    {
        foreach (GameObject go in tutorials)
            go.SetActive(true);
        yield return new WaitForSeconds(10);
        foreach (GameObject go in tutorials)
            go.SetActive(false);
    }

    private IEnumerator GameLoop()
    {
        while (gameOver == false)
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

            DamManager.Singleton.ChangeHealth();

            UpdateWater();
            
            yield return null;

            if (damCurrentHealth <= 0 || babiesCurrentFood <= 0)
            {
                gameOver = true;
            }
        }
        
        UIManager.Singleton.StopTimer();
        
        Debug.Log("Game over!");
    }

    private void UpdateWater()
    {
        float newY = Mathf.Lerp(maxY, minY, damCurrentHealth / damMaxHealth);
        Vector3 localPos = water.localPosition;
        localPos.y = newY;
        water.localPosition = localPos;
    }

    public void RespawnTree(Transform respawn)
    {
        Instantiate(treePrefab, respawn.position, respawn.rotation);
    }
}

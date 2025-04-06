using System;
using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;

    [Header("Game Manager")]
    public float damMaxHealth;
    [ReadOnly]
    public float damCurrentHealth;

    public float babiesMaxFood;
    [ReadOnly]
    public float babiesCurrentFood;

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
    public Transform damGameOver;
    public Transform kidsGameOver;
    [Space]
    public Transform water;
    public float minY = -0.4f;
    public float maxY = 0.7f;
    public Transform deadBABIES;

    [Header("GameOver")]
    public bool gameOver;
    public float maxWaterLevelTime = 1.5f;
    public float gameOverWaterLevel = 8f;

    private Transform mainCamera;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton.gameObject);
        }

        Singleton = this;

        //Baby's health set voor main scherm
        babiesCurrentFood = babiesMaxFood;
    }

    private void Start()
    {
        mainCamera = Camera.main.transform;
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

            babiesCurrentFood = Mathf.Clamp(babiesCurrentFood, 0, babiesMaxFood);
            damCurrentHealth = Mathf.Clamp(damCurrentHealth, 0, damMaxHealth);

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

        // Wanneer je Game Over bent
        UIManager.Singleton.StopTimer();
        FindFirstObjectByType<PlayerController>().lockPlayer = true;

        yield return new WaitForFixedUpdate();

        if (damCurrentHealth <= 0)
        {
            UIManager.Singleton.reasonText.text = "Your land got flooded!";
            StartCoroutine(LerpTransform(mainCamera, damGameOver, 3));

            babiesCurrentFood = 0;

            // Water vliegt omhoog in 1.5 seconden naar maxY
            StartCoroutine(RaiseWaterRapidly(gameOverWaterLevel, maxWaterLevelTime));
        }

        else
        {
            UIManager.Singleton.reasonText.text = "The kids have died of hunger!";
            StartCoroutine(LerpTransform(mainCamera, kidsGameOver, 3));
        }
    }

    public IEnumerator LerpTransform(Transform subject, Transform target, float duration)
    {
        Vector3 startPosition = subject.position;
        Quaternion startRotation = subject.rotation;

        Vector3 endPosition = target.position;
        Quaternion endRotation = target.rotation;

        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            subject.position = Vector3.Lerp(startPosition, endPosition, t);
            subject.rotation = Quaternion.Lerp(startRotation, endRotation, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        subject.position = endPosition;
        subject.rotation = endRotation;

        UIManager.Singleton.gameOverText.SetActive(true);

        if (babiesCurrentFood <= 0)
        {
            KidsManager.Singleton.PlayAnimation("DIE", false);
        }

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    private IEnumerator RaiseWaterRapidly(float targetY, float duration)
    {
        Vector3 startPos = water.position;
        Vector3 endPos = new Vector3(startPos.x, targetY, startPos.z);
        Vector3 startPosbabies = deadBABIES.position;
        Vector3 endPosbabies = new Vector3(startPosbabies.x, -1.64f, startPosbabies.z);
        float elapsed = 0f;

        deadBABIES.gameObject.SetActive(true);
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            water.position = Vector3.Lerp(startPos, endPos, t);
            deadBABIES.position= Vector3.Lerp(startPosbabies, endPosbabies, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Zorg dat hij perfect eindigt
        water.position = endPos;
    }

}

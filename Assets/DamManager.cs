using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamManager : MonoBehaviour
{
    public static DamManager Singleton;


    public List<GameObject> logVisuals = new List<GameObject>();
    public List<GameObject> waterVisuals = new List<GameObject>();

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton.gameObject);
        }

        Singleton = this;
    }
    public void ChangeHealth()
    {
        float progress = Mathf.Clamp(GameManager.Singleton.damCurrentHealth, 0, 100); // clamp just in case

        // Logs (40 steps)
        int logSteps = 40;
        int currentLogStep = Mathf.FloorToInt((progress / 100f) * logSteps);

        for (int i = logVisuals.Count - 1; i >= 0; i--)
        {
            logVisuals[i].SetActive(i < currentLogStep);
        }

        // Water (20 steps)
        int waterSteps = 20;
        int currentWaterStep = Mathf.FloorToInt((progress / 100f) * waterSteps);

        for (int i = waterVisuals.Count - 1; i >= 0; i--)
        {
            waterVisuals[i].SetActive(i >= currentWaterStep);
        }

    }

}

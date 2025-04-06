using UnityEngine;
using UnityEngine.Audio;

public class IncreasingWaterSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxVolume, minimumPercentage = 0.5f;
    private void Update()
    {
        ChangeVolume2();
    }

    private void ChangeVolume()
    {
        float damHealthPercentage = GameManager.Singleton.damCurrentHealth / GameManager.Singleton.damMaxHealth;


        float calculatedVolume = 0f;

        calculatedVolume = 1f - Mathf.Clamp01(damHealthPercentage / minimumPercentage);

        Debug.Log(calculatedVolume);

        if (GameManager.Singleton.damCurrentHealth != 0)
        {
            calculatedVolume = 1f - damHealthPercentage;
        }


        //Debug.Log(damHealthPercentage);
        audioSource.volume = calculatedVolume * maxVolume;
    }

    private void ChangeVolume2()
    {

        float damHealthPercentage = GameManager.Singleton.damCurrentHealth / GameManager.Singleton.damMaxHealth;
        float damagePercent = 1f - damHealthPercentage;                // 0 = healthy, 1 = destroyed

        if (damHealthPercentage <= 0f)
        {
            audioSource.volume = 0f;
            return;
        }

        float calculatedVolume = 0f;

        if (damagePercent >= minimumPercentage)
        {
            // Remap damagePercent from [minimumPercentage, 1] to [0, 1]
            float normalized = (damagePercent - minimumPercentage) / (1f - minimumPercentage);
            calculatedVolume = Mathf.Clamp01(normalized);
        }

        //Debug.Log($"Health: {damHealthPercentage:F2}, Damage: {damagePercent:F2}, Volume: {calculatedVolume:F2}");

        audioSource.volume = calculatedVolume * maxVolume;


    }


}

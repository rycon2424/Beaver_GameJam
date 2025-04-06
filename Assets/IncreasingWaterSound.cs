using UnityEngine;

public class IncreasingWaterSound : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxVolume, minimumPercentage = 0.5f;
    private void Update()
    {
        ChangeVolume();
    }

    private void ChangeVolume()
    {
        float damHealthPercentage = GameManager.Singleton.damCurrentHealth / GameManager.Singleton.damMaxHealth;


        float calculatedVolume = 0;

        calculatedVolume = 1f - Mathf.Clamp01(damHealthPercentage / minimumPercentage);

        Debug.Log(calculatedVolume);

        if(GameManager.Singleton.damCurrentHealth != 0)
        {
            calculatedVolume =  1f - damHealthPercentage;
        }


        //Debug.Log(damHealthPercentage);
        audioSource.volume = calculatedVolume * maxVolume;
    }


}

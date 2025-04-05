using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundPool : MonoBehaviour
{
    public static SoundPool Singleton;

    [SerializeField] private AudioClip[] clips;

    private Dictionary<string, AudioClip> clipDictionary = new Dictionary<string, AudioClip>();
    
    private List<AudioSource> sources = new List<AudioSource>();
    
    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(Singleton.gameObject);
        }
        
        Singleton = this;
    }

    private void Start()
    {
        sources.AddRange(GetComponentsInChildren<AudioSource>());

        foreach (var c in clips)
        {
            clipDictionary.Add(c.name, c);
        }
    }

    /// <summary>
    /// for example "Wood Breaking" or "Wood Impact"
    /// </summary>
    /// <param name="audioClipName"></param>
    public void PlaySound(string audioClipName)
    {
        for (int i = 0; i < sources.Count; i++)
        {
            if (sources[i].isPlaying == false)
            {
                sources[i].Play();
                return;
            }
        }
    }
}
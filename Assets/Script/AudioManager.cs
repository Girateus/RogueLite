using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _Music;

    public AudioClip BackgroundMusic;
    public AudioClip BossMusic;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _Music.clip = BackgroundMusic;
        _Music.Play();
    }
    
    public void PlayBossMusic()
    {
         _Music.Stop();
         _Music.clip = BossMusic;
        _Music.Play(); 
    }
}


using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance {get; private set;}
    private AudioSource musicAudioSource;
    private float volume=0.4f;
    private void Awake()
    {
        Instance = this;
        musicAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, 0.4f);
        
    }

    public void ChangeVolume()
    {
        volume += 0.1f;
        if (volume > 1f)
        {
            volume = 0f;
        }

        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}

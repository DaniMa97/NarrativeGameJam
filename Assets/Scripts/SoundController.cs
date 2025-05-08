using UnityEngine;

public class SoundController : MonoBehaviour
{

    static SoundController instance;

    float masterVolume;
    float musicVolume;
    float sfxVolume;
    float voiceVolume;

    public static SoundController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        print(value);
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        print(value);
    }
    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
        print(value);
    }
    public void SetVoiceVolume(float value)
    {
        voiceVolume = value;
        print(value);
    }
}

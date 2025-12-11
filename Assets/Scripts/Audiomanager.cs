using System;
using UnityEngine;


[Serializable]
public class SoundTypeclass
{
    public SoundsEnum soundType;
    public AudioClip soundClip;
}
public class Audiomanager : MonoBehaviour
{
    public AudioSource SoundEffect;
    public AudioSource SoundMusic;
    public bool IsMute = false;
    public float Volume = 1.0f;
    public SoundTypeclass[] Sounds;

    private static Audiomanager instance;
    public static Audiomanager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        SetVolume(0.5f);
        playMusic(SoundsEnum.Music);
    }

    public void mute(bool status)
    {
        IsMute = status;
    }
    public void SetVolume(float volume)
    {
        Volume = volume;
        SoundEffect.volume = Volume;
        SoundMusic.volume = Volume;
    }
    public void playMusic(SoundsEnum sound)
    {
        if (IsMute)
            return;

        AudioClip clip = getAudioClip(sound);
        if (clip != null)
        {
            SoundMusic.clip = clip;
            SoundMusic.Play();
        }
        else
        {
            Debug.LogError("AudioClip not found for type :" + sound);
        }
    }
    public void play(SoundsEnum sound)
    {
        AudioClip clip = getAudioClip(sound);
        if (clip != null)
        {
            SoundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("AudioClip not found for type :" + sound);
        }
    }

    private AudioClip getAudioClip(SoundsEnum sound)
    {
        SoundTypeclass item = Array.Find(Sounds, i => i.soundType == sound);
        if (item != null)
            return item.soundClip;
        return null;
    }


}

public enum SoundsEnum
{
    ButtonClick,
    Food,
    PowerUp,
    PlayerDead,
    Music,
}




using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sound
{
    BIRD_JUMP,
    BIRD_DIE,
    SCORE,
    BUTTON_HOVER,
    BUTTON_CLICK
}

public class SoundManager : MonoBehaviour
{
    [System.Serializable]
    private struct SoundClip
    {
        public Sound sound;
        public AudioClip clip;
    }

    #region Inspector Fields
    [SerializeField]
    private List<SoundClip> SoundClipList;

    [SerializeField]
    private AudioSource EffectsSource;

    [SerializeField]
    private AudioSource MusicSource;
    #endregion

    private bool m_Muted;

    //Instance
    private static SoundManager m_Instance = null;

    private void Awake()
    {
        if (!m_Instance) m_Instance = this;
        else if (m_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    #region Public Methods
    public static SoundManager GetInstance()
    {
        return m_Instance;
    }

    public void Mute(bool mute = true)
    {
        m_Muted = mute;
        if(mute)
        {
            EffectsSource.Stop();
            MusicSource.Stop();
        }
        else
        {
            MusicSource.Play();
        }
    }

    public void PlaySoundEffect(Sound sound)
    {
        if (m_Muted) return;

        EffectsSource.PlayOneShot(GetAudioClip(sound));
    }
    #endregion

    private AudioClip GetAudioClip(Sound sound)
    {
        foreach(SoundClip soundClip in SoundClipList)
        {
            if (soundClip.sound == sound) return soundClip.clip;
        }

        //Clip not found
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }
}

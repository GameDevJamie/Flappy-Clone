using System;
using UnityEngine;

public enum EGameSpeed { SLOW = 1, NORMAL = 2, FAST = 3, VERY_FAST = 4 }
public enum EGravityStrength {WEAK = 1, NORMAL = 2, STRONG = 3 }
public enum EMirrorMode { OFF = 0, HORIZONTAL = 1, VERTICAL = 2, BOTH = 3}

public static class GameSettings
{
    #region Setters
    public static void SetGameSpeed(EGameSpeed speed)
    {
        float timeScale = ((int)speed * 0.5f);
        Time.timeScale = timeScale;
        PlayerPrefs.SetString("GameSpeed", speed.ToString());
    }

    public static void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public static void SetGravityStrength(EGravityStrength gravity)
    {
        PlayerPrefs.SetString("GravityStrength", gravity.ToString());
    }

    public static void SetMirrorMode(EMirrorMode mode)
    {
        PlayerPrefs.SetString("MirrorMode", mode.ToString());
    }
    #endregion


    #region Getters
    public static EGameSpeed GetGameSpeed()
    {
        string speed = PlayerPrefs.GetString("GameSpeed", "NORMAL");
        return (EGameSpeed)Enum.Parse(typeof(EGameSpeed), speed);
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    public static EGravityStrength GetGravityStrength()
    {
        string gravity = PlayerPrefs.GetString("GravityStrength", "NORMAL");
        return (EGravityStrength)Enum.Parse(typeof(EGravityStrength), gravity);
    }

    public static EMirrorMode GetMirrorMode()
    {
        string mode = PlayerPrefs.GetString("MirrorMode", "OFF");
        return (EMirrorMode)Enum.Parse(typeof(EMirrorMode), mode);
    }
    #endregion
}
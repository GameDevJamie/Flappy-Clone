using System;
using UnityEngine;

public enum EModType
{
    WEIGHT = 0,
    SHIFT = 1,
    MIRROR = 2
    //SPEED = 3
}

public static class ModManager
{
    /// <summary>
    /// Save given mod as active
    /// </summary>
    /// <param name="type"></param>
    /// <param name="active"></param>
    public static void SetModActive(EModType type, bool active = true)
    {
        PlayerPrefs.SetInt(GetModName(type), active ? 1 : 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Check if given mod type has been set to active
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsModActive(EModType type)
    {
        return (PlayerPrefs.GetInt(GetModName(type), 0) == 1);
    }

    /// <summary>
    /// Resets all Mods back to InActive
    /// </summary>
    public static void ResetActiveMods()
    {
        //Get enum length
        int length = GetNumMods();

        for (int i = 0; i < length; ++i)
        {
            //Convert int to enum
            EModType type = (EModType)i;
            PlayerPrefs.SetInt(GetModName(type), 0);
        }

        PlayerPrefs.Save();
    }

    public static int GetNumMods()
    {
        return Enum.GetNames(typeof(EModType)).Length;
    }

    private static string GetModName(EModType type)
    {
        return type.ToString() + "_Mod";
    }
}

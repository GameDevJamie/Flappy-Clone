using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    /// <summary>
    /// Retrieves current highscore saved
    /// </summary>
    /// <returns></returns>
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore");
    }

    /// <summary>
    /// Sets a new highscore if the given score is higher
    /// </summary>
    /// <param name="score"></param>
    /// <returns></returns>
    public static bool TrySetNewHighScore(int score)
    {
        int current = GetHighScore();
        if (score > current)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            return true;
        }
        else return false;
    }

    /// <summary>
    /// Reset current highscore back to zero
    /// </summary>
    /// <returns></returns>
    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
    }
}

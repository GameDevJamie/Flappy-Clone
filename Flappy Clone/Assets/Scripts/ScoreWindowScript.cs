using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindowScript : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;

    private int _currentScore;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnPipePassed += new System.EventHandler<int>(UpdateScoreText);
        ResetScore();
    }

    private void UpdateScoreText(object sender, int pipesPassed)
    {
        ScoreText.text = "Score: " + pipesPassed;
        _currentScore = pipesPassed;
        Score.SetScore(_currentScore);

        SoundManager.GetInstance().PlaySoundEffect(Sound.SCORE);
    }

    private void ResetScore()
    {
        ScoreText.text = "Score: 0";
    }
}

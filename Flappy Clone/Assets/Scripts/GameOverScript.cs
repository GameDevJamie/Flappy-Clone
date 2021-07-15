using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;
    [SerializeField]
    private Text HighScoreTitleText;
    [SerializeField]
    private Text HighScoreText;

    // Start is called before the first frame update
    void Start()
    {
        var levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        transform.Find("RetryBtn").GetComponent<Button>().onClick.AddListener(delegate { levelLoader.StartGame(); } );
        transform.Find("MenuBtn").GetComponent<Button>().onClick.AddListener(delegate { levelLoader.MainMenu(); });

        UpdateScore();
    }

    private void UpdateScore()
    {
        int score = Score.GetScore();
        int highScore = Score.GetHighScore();

        HighScoreTitleText.text = "HighScore";

        ScoreText.text = score.ToString();
        HighScoreText.text = highScore.ToString();

        if (score > highScore)
        {
            HighScoreTitleText.text = "NEW HIGHSCORE!";

            Score.TrySetNewHighScore(score);
            HighScoreText.text = score.ToString();
        }
    }
}

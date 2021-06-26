using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindowScript : MonoBehaviour
{
    [SerializeField]
    private Text ScoreText;
    //[SerializeField]
    //private Text HighScoreText;

    private int m_Multiplier;

    private void Awake()
    {
        m_Multiplier = 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnPipePassed += new System.EventHandler<int>(UpdateScoreText);
        ResetScore();
    }

    private void UpdateScoreText(object sender, int pipesPassed)
    {
        ScoreText.text = "Score: " + (pipesPassed * m_Multiplier);

        SoundManager.GetInstance().PlaySoundEffect(Sound.SCORE);
    }

    private void ResetScore()
    {
        ScoreText.text = "Score: 0";
    }
}

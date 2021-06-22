using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    public Text ScoreText;

    private AudioSource m_ScoreSound;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnPipePassed += new System.EventHandler<int>(UpdateScoreText);

        m_ScoreSound = GetComponent<AudioSource>();

        ResetScore();
    }

    private void UpdateScoreText(object sender, int pipesPassed)
    {
        ScoreText.text = "Score: " + pipesPassed;
        m_ScoreSound.Play();
    }

    private void ResetScore()
    {
        ScoreText.text = "Score: 0";
    }
}

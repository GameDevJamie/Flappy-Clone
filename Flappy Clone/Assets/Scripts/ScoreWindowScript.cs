using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindowScript : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> Numbers;
    [SerializeField]
    private Vector2 NumberSize;
    [SerializeField]
    private float XOffset;

    private int _currentScore;
    private List<Image> SpawnedNumbers;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnPipePassed += new System.EventHandler<int>(UpdateScore);
        ResetScore();

        SpawnedNumbers = new List<Image>();
        SpawnNumberSprite();    //Spawn first number
    }

    private void UpdateScore(object sender, int pipesPassed)
    {
        _currentScore = pipesPassed;

        UpdateNumberSprites();

        Score.SetScore(_currentScore);
        SoundManager.GetInstance().PlaySoundEffect(Sound.SCORE);
    }

    private void ResetScore()
    {
        //ScoreText.text = "Score: 0";
    }

    private void SpawnNumberSprite()
    {
        GameObject g = new GameObject("Score_Number", typeof(Image));

        RectTransform transform = g.GetComponent<RectTransform>();
        transform.SetParent(this.gameObject.transform);
        transform.anchoredPosition = Vector3.up * 500f;
        transform.localScale = Vector3.one;
        transform.sizeDelta = new Vector2(NumberSize.x, NumberSize.y);

        Image image = g.GetComponent<Image>();
        image.sprite = Numbers[0];

        SpawnedNumbers.Add(image);

        //Set new positions
        float xPos = (SpawnedNumbers.Count - 1) * (-XOffset * 0.5f);
        for(int i = 0; i < SpawnedNumbers.Count; ++i)
        {
            Vector2 newPos = new Vector2(xPos, 500f);
            SpawnedNumbers[i].rectTransform.anchoredPosition = newPos;

            xPos += XOffset;
        }
    }

    private void UpdateNumberSprites()
    {
        //Split the score into strings
        //For each string, set the right numbers
        string scoreString = _currentScore.ToString();

        //Spawn new Number if needed
        if (scoreString.Length > SpawnedNumbers.Count) SpawnNumberSprite();

        for (int i = 0; i < scoreString.Length; ++i)
        {
            int score = (int)Char.GetNumericValue(scoreString[i]);
            SpawnedNumbers[i].sprite = Numbers[score];
        }
    }
}

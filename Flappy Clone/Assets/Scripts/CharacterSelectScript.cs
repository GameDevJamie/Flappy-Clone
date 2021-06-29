using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScript : MonoBehaviour
{
    /// <summary>
    /// Sprite to change
    /// </summary>
    [SerializeField]
    private SpriteRenderer Sprite;

    /// <summary>
    /// List of sprites to change to
    /// </summary>
    [SerializeField]
    private List<Sprite> SpriteList;

    private AudioSource AudioSource;

    private int m_SelectedSpriteIndex;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("CharacterSelectIndex", 0);

        //Set Sprite to previously used bird
        Sprite.sprite = SpriteList[selectedIndex];
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousSprite();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            PreviousSprite();
        }
    }

    public void NextSprite()
    {
        m_SelectedSpriteIndex++;
        if (m_SelectedSpriteIndex >= SpriteList.Count) m_SelectedSpriteIndex = 0;

        Sprite.sprite = SpriteList[m_SelectedSpriteIndex];

        PlaySound();
    }

    public void PreviousSprite()
    {
        m_SelectedSpriteIndex--;
        if (m_SelectedSpriteIndex < 0) m_SelectedSpriteIndex = (SpriteList.Count - 1);

        Sprite.sprite = SpriteList[m_SelectedSpriteIndex];

        PlaySound();
    }

    private void PlaySound()
    {
        AudioSource.Play();
    }
}

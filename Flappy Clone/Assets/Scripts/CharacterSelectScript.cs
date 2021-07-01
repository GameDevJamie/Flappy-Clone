using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectScript : MonoBehaviour
{
    /// <summary>
    /// Sprite/Animation to change
    /// </summary>
    [SerializeField]
    private Animator Sprite;

    [SerializeField]
    private int m_NumAnimations = 3;

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
        m_SelectedSpriteIndex = selectedIndex;
        SetAnimation(m_SelectedSpriteIndex);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousSprite();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextSprite();
        }
    }

    public void NextSprite()
    {
        m_SelectedSpriteIndex++;
        if (m_SelectedSpriteIndex >= m_NumAnimations) m_SelectedSpriteIndex = 0;

        SetAnimation(m_SelectedSpriteIndex);
        PlaySound();
    }

    public void PreviousSprite()
    {
        m_SelectedSpriteIndex--;
        if (m_SelectedSpriteIndex < 0) m_SelectedSpriteIndex = m_NumAnimations - 1;

        SetAnimation(m_SelectedSpriteIndex);
        PlaySound();
    }

    private void PlaySound()
    {
        AudioSource.Play();
    }

    private void SetAnimation(int index)
    {
        if (index >= m_NumAnimations) return;
        Sprite.SetInteger("AnimIndex", index);
    }
}

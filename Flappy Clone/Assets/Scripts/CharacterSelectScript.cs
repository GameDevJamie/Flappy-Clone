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

    private int m_SelectedSpriteIndex;

    private void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("CharacterSelectIndex", 0);

        //Set Sprite to previously used bird
        Sprite.sprite = SpriteList[selectedIndex];
    }

    public void NextSprite()
    {
        m_SelectedSpriteIndex++;
        if (m_SelectedSpriteIndex >= SpriteList.Count) m_SelectedSpriteIndex = 0;

        Sprite.sprite = SpriteList[m_SelectedSpriteIndex];
    }

    public void PreviousSprite()
    {
        m_SelectedSpriteIndex--;
        if (m_SelectedSpriteIndex < 0) m_SelectedSpriteIndex = (SpriteList.Count - 1);

        Sprite.sprite = SpriteList[m_SelectedSpriteIndex];
    }
}

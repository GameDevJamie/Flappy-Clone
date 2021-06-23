using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectScript : MonoBehaviour
{
    //Possible Bird Sprites to select
    public List<Sprite> SpriteList;

    private GameObject m_Bird;
    private int m_SelectedSpriteIndex;

    // Start is called before the first frame update
    void Start()
    {
        m_Bird = GameObject.FindGameObjectWithTag("Player");
    }

    public void NextSprite()
    {
        m_SelectedSpriteIndex++;
        if (m_SelectedSpriteIndex >= SpriteList.Count) m_SelectedSpriteIndex = 0;

        m_Bird.GetComponent<SpriteRenderer>().sprite = SpriteList[m_SelectedSpriteIndex];
    }

    public void PreviousSprite()
    {
        m_SelectedSpriteIndex--;
        if (m_SelectedSpriteIndex < 0) m_SelectedSpriteIndex = (SpriteList.Count - 1);

        m_Bird.GetComponent<SpriteRenderer>().sprite = SpriteList[m_SelectedSpriteIndex];
    }
}

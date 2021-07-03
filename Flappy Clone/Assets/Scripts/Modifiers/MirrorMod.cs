using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMod : ModifierBase
{
    private float m_OriginalSize;

    private void Start()
    {
        m_OriginalSize = Camera.main.orthographicSize;
        Camera.main.orthographicSize = -m_OriginalSize;

        //Flip UI Components too
        GameObject.FindGameObjectWithTag("UI").GetComponent<RectTransform>().localScale = new Vector3(-1, -1, 1);
    }

    private void OnDestroy()
    {
        if(Camera.main) Camera.main.orthographicSize = m_OriginalSize;

        //Reset UI Scale
        var ui = GameObject.FindGameObjectWithTag("UI");
        if(ui) ui.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }
}

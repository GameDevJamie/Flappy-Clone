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
    }

    private void OnDestroy()
    {
        Camera.main.orthographicSize = m_OriginalSize;
    }
}

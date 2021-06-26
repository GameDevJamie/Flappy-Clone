using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightMod : ModifierBase
{
    private GameObject m_Bird;
    private float m_OriginalGravity;
    private float m_AlteredGravity;

    private void Awake()
    {
        m_AlteredGravity = 36.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Bird = GameObject.FindGameObjectWithTag("Player");
        m_OriginalGravity = m_Bird.GetComponent<Rigidbody2D>().gravityScale;
        m_Bird.GetComponent<Rigidbody2D>().gravityScale = m_AlteredGravity;
    }

    private void OnDestroy()
    {
        //Reset Gravity
        if (!m_Bird)
        {
            m_Bird.GetComponent<Rigidbody2D>().gravityScale = m_OriginalGravity;
        }
    }
}

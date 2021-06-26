using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBase : MonoBehaviour
{
    private float m_Timer;
    private bool m_IsTimed;

    // Update is called once per frame
    void Update()
    {
        if (m_IsTimed)
        {
            m_Timer -= Time.deltaTime;
            if (m_Timer < 0.0f) Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Sets the timer for this modifier, destroying it once the timer reaches zero
    /// </summary>
    /// <param name="timer"></param>
    public void SetTimer(float timer)
    {
        m_Timer = timer;
        m_IsTimed = true;
    }
}

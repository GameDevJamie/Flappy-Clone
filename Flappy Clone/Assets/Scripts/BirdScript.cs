using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public float JumpSpeed;
    public float RotSpeed;

    private enum EState { IDLE, PLAYING, DEAD }
    private EState m_State;

    private Rigidbody2D m_RigidBody;

    //Events
    public event EventHandler OnStartedPlaying;
    public event EventHandler OnDied;

    private void Awake()
    {
        m_State = EState.IDLE;
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_RigidBody.bodyType = RigidbodyType2D.Static;  //Sit Still
    }

    private void Update()
    {
        bool jumpKey = (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        switch (m_State)
        {
            case EState.IDLE:
                if(jumpKey)
                {
                    m_RigidBody.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if(OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                    m_State = EState.PLAYING;
                }
                break;

            case EState.PLAYING:
                if (jumpKey) Jump();

                this.transform.eulerAngles = new Vector3(0.0f, 0.0f, m_RigidBody.velocity.y * 0.3f);
                break;

            case EState.DEAD:
                break;
        }
    }

    /// <summary>
    /// Jump Bird
    /// </summary>
    private void Jump()
    {
        m_RigidBody.velocity = Vector3.up * JumpSpeed;
    }

    /// <summary>
    /// Check Collisions
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Obstacle")
        {
            if(m_State != EState.DEAD)
            {
                //Call Died Event
                if (OnDied != null) OnDied(this, EventArgs.Empty);
            }

            m_State = EState.DEAD;
            m_RigidBody.velocity = Vector3.down;    //Start Falling
        }
        else if(col.tag == "Ground")
        {
            if (m_State != EState.DEAD)
            {
                //Call Died Event
                if (OnDied != null) OnDied(this, EventArgs.Empty);
            }

            m_State = EState.DEAD;
            m_RigidBody.bodyType = RigidbodyType2D.Static;  //Sit Still
        }
    }
}

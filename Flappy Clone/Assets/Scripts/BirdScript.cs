using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public float JumpSpeed;
    public float RotSpeed;

    private enum EState { IDLE, JUMPING, DEAD }
    private EState m_State;

    private Rigidbody2D m_RigidBody;
    private Animator m_Animator;
    private int m_PipesPassed;

    //Events
    public event EventHandler OnDied;
    public event EventHandler<int> OnPipePassed;

    private void Awake()
    {
        m_State = EState.IDLE;
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_RigidBody.bodyType = RigidbodyType2D.Static;  //Sit Still

        //Get Animator
        m_Animator = GetComponent<Animator>();

        m_PipesPassed = 0;
    }

    private void Update()
    {
        bool jumpKey = (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0));
        switch (m_State)
        {
            case EState.IDLE:
                break;

            case EState.JUMPING:
                if (jumpKey) Jump();

                this.transform.eulerAngles = new Vector3(0.0f, 0.0f, m_RigidBody.velocity.y * 0.3f);

                //Check if out of bounds
                if (this.transform.position.y > Camera.main.orthographicSize + 5.0f)
                {
                    Die();
                }
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
        SoundManager.GetInstance().PlaySoundEffect(Sound.BIRD_JUMP);
    }

    private void Die()
    {
        m_State = EState.DEAD;
        m_Animator.SetBool("Dead", true);
        m_RigidBody.velocity = Vector3.down;    //Start Falling

        if (OnDied != null) OnDied(this, EventArgs.Empty);

        SoundManager.GetInstance().PlaySoundEffect(Sound.BIRD_DIE);
    }

    /// <summary>
    /// Check Collisions
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Ground" && m_State == EState.DEAD)
        {
            m_RigidBody.bodyType = RigidbodyType2D.Static;
            return;
        }
        if (m_State == EState.DEAD) return;


        //Not Dead
        //------------
        if(col.tag == "Obstacle")
        {
            Die();
        }
        else if(col.tag == "Ground")
        {
           Die();
           m_RigidBody.bodyType = RigidbodyType2D.Static;  //Sit Still
        }
        else if (col.tag == "Score")
        {
            m_PipesPassed++;
            if (OnPipePassed != null) OnPipePassed(this, m_PipesPassed);
        }
        //------------
    }


    #region Public Methods
    public void StartJumping()
    {
        m_RigidBody.bodyType = RigidbodyType2D.Dynamic;
        Jump();
        m_State = EState.JUMPING;
    }

    public int GetPipesPassed()
    {
        return m_PipesPassed;
    }
    #endregion
}

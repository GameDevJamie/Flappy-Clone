using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets m_Instance;

    public static GameAssets GetInstance()
    {
        return m_Instance;
    }

    private void Awake()
    {
        m_Instance = this;
    }

    public Transform pfPipeHead;
    public Transform pfPipeBody;
    public float CameraOrthoSize;

    //Audio
    public AudioClip sndScore;
    public AudioClip sndBirdJump;
    public AudioClip sndBirdDie;
    public AudioClip sndButtonHover;
}

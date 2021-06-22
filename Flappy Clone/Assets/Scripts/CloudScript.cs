using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    public List<Sprite> m_SpriteList;   //List of Sprites/Clouds that can be spawned
    public float MoveSpeed;             //How fast to move the clouds
    public float SpawnTimerMin;
    public float SpawnTimerMax;

    private float m_DestroyXPos;    //When to destroy cloud objects
    private float m_SpawnXPos;      //Where on X-Axis to spawn new Clouds

    private float m_SpawnTimer;

    private bool m_Active;

    private List<Cloud> m_Clouds;

    private void Awake()
    {
        m_Clouds = new List<Cloud>();
        m_Active = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        float cameraOrthoSize = (GameAssets.GetInstance().CameraOrthoSize * 2.0f);

        m_SpawnXPos = cameraOrthoSize + 20.0f;
        m_DestroyXPos = -cameraOrthoSize - 20.0f;

        m_SpawnTimer = Random.Range(SpawnTimerMin, SpawnTimerMax);

        //Spawn Some Clouds to start with
        int numToSpawn = 3;
        float offsetX = (cameraOrthoSize / numToSpawn);

        float xPos = (-cameraOrthoSize + offsetX);
        while(numToSpawn != 0)
        {
            SpawnCloud(xPos, 35.0f);

            xPos += (offsetX * 2f);
            numToSpawn--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Active) return;

        m_SpawnTimer -= Time.deltaTime;
        if(m_SpawnTimer < 0f)
        {
            SpawnCloud(m_SpawnXPos, 35.0f);
            m_SpawnTimer = Random.Range(SpawnTimerMin, SpawnTimerMax);
        }

        MoveClouds();
    }

    private void SpawnCloud(float xPos, float yPos)
    {
        //Random Index
        int index = Random.Range(0, m_SpriteList.Count);

        //Create GameObject
        GameObject g = new GameObject("Cloud");
        g.transform.position = new Vector3(xPos, yPos);

        //Add Sprite Component
        SpriteRenderer sprite = g.AddComponent<SpriteRenderer>();
        sprite.sprite = m_SpriteList[index];
        sprite.sortingOrder = 1;
        sprite.sortingLayerName = "Background";

        g.transform.SetParent(this.transform);

        m_Clouds.Add(new Cloud(g.transform));
    }

    private void MoveClouds()
    {
        for(int i = m_Clouds.Count - 1; i >=0 ; --i)
        {
            m_Clouds[i].Move(MoveSpeed);
            if(m_Clouds[i].GetXPos() < m_DestroyXPos)
            {
                m_Clouds[i].DestroySelf();
                m_Clouds.RemoveAt(i);
            }
        }
    }

    #region Public Methods
    public void Enable(bool enable = true)
    {
        m_Active = enable;
    }
    #endregion

    private class Cloud
    {
        private Transform m_Transform;

        public Cloud(Transform t)
        {
            m_Transform = t;
        }

        public float GetXPos()
        {
            return m_Transform.position.x;
        }

        public void Move(float moveSpeed)
        {
            m_Transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        }

        public void DestroySelf()
        {
            Destroy(m_Transform.gameObject);
        }
    }
}

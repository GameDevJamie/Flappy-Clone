using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GapPipeScript : MonoBehaviour
{
    public float PipeMoveSpeed = 10.0f;
    public float PipeSpawnXPos = 120.0f;
    public float PipeDestroyXPos = -120.0f;

    private Pipe TopPipe = null;
    private Pipe BottomPipe = null;

    private bool m_Move;
    private float m_GapY;
    private float m_GapSize;
    private bool m_UpdateGapSize;
    

    private void Awake()
    {
        m_Move = true;
        m_GapY = 0f;
        m_GapSize = 0f;
        m_UpdateGapSize = false;

        CreateGapPipes(m_GapY, m_GapSize, PipeSpawnXPos);
    }

    private void Start()
    {
        if (m_UpdateGapSize)
        {
            UpdateGapPipes(m_GapY, m_GapSize);
            m_UpdateGapSize = false;
        }

        //Subscribe to Events
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnDied += Bird_Died;
    }

    private void Bird_Died(object sender, System.EventArgs a)
    {
        m_Move = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_Move) return;

        if(m_UpdateGapSize)
        {
            //Todo: Transition in real-time between the different gap sizes
            UpdateGapPipes(m_GapY, m_GapSize);
            m_UpdateGapSize = false;
        }

        TopPipe.Move(PipeMoveSpeed * Time.deltaTime);
        BottomPipe.Move(PipeMoveSpeed * Time.deltaTime);

        //Only need to test one pipe to destroy both, since they are at the same X Position
        if(TopPipe.GetXPos() < PipeDestroyXPos)
        {
            Destroy(this.gameObject);
        }
    }


    private void OnDestroy()
    {
        if (BottomPipe != null) BottomPipe.DestroySelf();
        if (TopPipe != null) TopPipe.DestroySelf();
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPos)
    {
        if (BottomPipe != null) BottomPipe.DestroySelf();
        if (TopPipe != null) TopPipe.DestroySelf();

        float gapSizeHalf = gapSize * 0.5f;

        BottomPipe = new Pipe(gapY - gapSizeHalf, xPos, true);
        TopPipe = new Pipe((GameAssets.GetInstance().CameraOrthoSize * 2.0f) - gapY - gapSizeHalf, xPos, false);

        BottomPipe.SetParent(this.transform);
        TopPipe.SetParent(this.transform);
    }

    private void UpdateGapPipes(float gapY, float gapSize)
    {
        if (BottomPipe == null || TopPipe == null) return;

        float gapSizeHalf = gapSize * 0.5f;
        BottomPipe.SetHeight(gapY - gapSizeHalf);
        TopPipe.SetHeight((GameAssets.GetInstance().CameraOrthoSize * 2.0f) - gapY - gapSizeHalf);
    }

    public void SetGapSize(float gapY, float gapSize)
    {
        m_GapY = gapY;
        m_GapSize = gapSize;
        m_UpdateGapSize = true;
    }



    /// <summary>
    /// Represents a Single Pipe
    /// </summary>
    private class Pipe
    {
        private float CAMERA_ORTHO_SIZE = GameAssets.GetInstance().CameraOrthoSize;

        private Transform m_PipeHead;
        private Transform m_PipeBody;
        private bool m_IsBottom;

        #region Constructor
        public Pipe(float height, float xPos, bool isBottom)
        {
            m_IsBottom = isBottom;

            CreatePipeHead(height, xPos);
            CreatePipeBody(height, xPos);
        }
        #endregion

        #region Create
        private void CreatePipeHead(float height, float xPos)
        {
            m_PipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
            m_PipeHead.position = new Vector3(xPos, 0f);

            UpdatePipeHead(height);
        }

        private void CreatePipeBody(float height, float xPos)
        {
            m_PipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
            m_PipeBody.position = new Vector3(xPos, 0f);

            UpdatePipeBody(height);
        }
        #endregion

        #region Update
        private void UpdatePipeHead(float height)
        {
            float headHeight = m_PipeHead.GetComponent<SpriteRenderer>().size.y;

            //Set if Bottom or Top
            float yPos = -CAMERA_ORTHO_SIZE + height - (headHeight * 0.5f);
            if (!m_IsBottom) yPos = CAMERA_ORTHO_SIZE - height + (headHeight * 0.5f);

            m_PipeHead.position = new Vector3(m_PipeHead.position.x, yPos);
        }

        private void UpdatePipeBody(float height)
        {
            float yPos = -CAMERA_ORTHO_SIZE;
            if (!m_IsBottom)
            {
                yPos = CAMERA_ORTHO_SIZE;
                m_PipeBody.localScale = new Vector3(1, -1, 1);
            }
            m_PipeBody.position = new Vector3(m_PipeBody.position.x, yPos);

            SpriteRenderer sr = m_PipeBody.GetComponent<SpriteRenderer>();
            float pipeWidth = sr.size.x;
            sr.size = new Vector2(pipeWidth, height);

            BoxCollider2D bc = m_PipeBody.GetComponent<BoxCollider2D>();
            bc.size = new Vector2(pipeWidth, height);
            bc.offset = new Vector2(0.0f, height * 0.5f);
        }
        #endregion

        #region Public Methods
        public void Move(float moveSpeed)
        {
            m_PipeHead.position += Vector3.left * moveSpeed;
            m_PipeBody.position += Vector3.left * moveSpeed;
        }

        public float GetXPos()
        {
            return m_PipeHead.position.x;
        }

        public void SetHeight(float height)
        {
            UpdatePipeHead(height);
            UpdatePipeBody(height);
        }

        public void SetParent(Transform parent)
        {
            m_PipeHead.SetParent(parent);
            m_PipeBody.SetParent(parent);
        }

        public void DestroySelf()
        {
            Destroy(m_PipeHead.gameObject);
            Destroy(m_PipeBody.gameObject);
        }
        #endregion
    }
}
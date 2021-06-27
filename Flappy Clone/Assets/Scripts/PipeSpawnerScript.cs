using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PipeSpawnerScript : MonoBehaviour
{
    [System.Serializable]
    public struct Difficulty
    {
        public float PipeSpawnRate; //How fast to spawn pipes in seconds
        public float GapSize;       //How wide to make the gap
        public float PipesSpawned;  //How many Pipes to have spawned before switching to this Difficulty
    }

    public float MoveSpeed;
    public List<Difficulty> DifficultyList;
    private int m_DifficultyIndex;

    private int m_PipesSpawned;
    private float PipeSpawnTimer;

    private float m_SpawnXPos;
    private float m_DestroyXPos;
    private List<GapPipe> m_GapPipesList;

    private bool m_Active;


    private void Awake()
    {
        m_DifficultyIndex = 0;
        m_PipesSpawned = 0;
        PipeSpawnTimer = DifficultyList[m_DifficultyIndex].PipeSpawnRate;

        m_GapPipesList = new List<GapPipe>();

        m_Active = false;
    }

    private void Start()
    {
        m_SpawnXPos = Mathf.Abs((Camera.main.orthographicSize * 2.0f)) + 20.0f;
        m_DestroyXPos = -m_SpawnXPos;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_Active) return;
        HandlePipeSpawn();
        MovePipes();
    }

    private void HandlePipeSpawn()
    {
        PipeSpawnTimer -= Time.deltaTime;
        if(PipeSpawnTimer <= 0.0f)
        {
            //Calculate Min and Max height for the pipes
            float gapSizeHalf = DifficultyList[m_DifficultyIndex].GapSize * 0.5f;
            float heightEdgeLimit = 10f;
            float minHeight = gapSizeHalf + heightEdgeLimit;
            float totalHeight = Mathf.Abs((Camera.main.orthographicSize * 2.0f));
            float maxHeight = totalHeight - gapSizeHalf - heightEdgeLimit;

            //Spawn Pipe
            float height = Random.Range(minHeight, maxHeight);

            SpawnGapPipe(height, DifficultyList[m_DifficultyIndex].GapSize, m_SpawnXPos);


            UpdateDifficulty(); //Update Difficulty now to get new spawn rate
            PipeSpawnTimer = DifficultyList[m_DifficultyIndex].PipeSpawnRate;
        }
    }

    private void MovePipes()
    {
        for (int i = m_GapPipesList.Count - 1; i >= 0; --i)
        {
            m_GapPipesList[i].Move(MoveSpeed * Time.deltaTime);
            if (m_GapPipesList[i].GetXPos() < m_DestroyXPos)
            {
                m_GapPipesList[i].DestroySelf();
                m_GapPipesList.RemoveAt(i);
            }
        }
    }

    private void SpawnGapPipe(float gapY, float gapSize, float xPos)
    {
        m_GapPipesList.Add(new GapPipe(gapY, gapSize, xPos));
        m_PipesSpawned++;
    }

    private void UpdateDifficulty()
    {
        if (m_DifficultyIndex == DifficultyList.Count - 1) return;  //Reached end of list
        if (m_PipesSpawned >= DifficultyList[m_DifficultyIndex + 1].PipesSpawned) m_DifficultyIndex++;
    }


    #region Public Methods
    public void Enable(bool enable = true)
    {
        m_Active = enable;
    }
    #endregion

    private class GapPipe
    {
        private Pipe m_TopPipe;
        private Pipe m_BottomPipe;
        private GameObject m_ScoreTrigger;

        #region Constructor
        public GapPipe(float gapY, float gapSize, float xPos)
        {
            float orthoSize = Mathf.Abs(Camera.main.orthographicSize);
            float gapSizeHalf = (gapSize * 0.5f);

            m_BottomPipe = new Pipe(gapY - gapSizeHalf, xPos, true);
            m_TopPipe = new Pipe((orthoSize * 2.0f) - gapY - gapSizeHalf, xPos, false);

            //Create Score Trigger
            m_ScoreTrigger = new GameObject("Score Trigger");
            m_ScoreTrigger.tag = "Score";
            m_ScoreTrigger.transform.position = new Vector3(xPos + 5.0f, 0);
            BoxCollider2D box = m_ScoreTrigger.AddComponent<BoxCollider2D>();
            box.size = new Vector2(2.0f, orthoSize * 2f);
        }
        #endregion

        #region Public Methods
        public float GetXPos()
        {
            //Both pipes at same X Position
            return m_TopPipe.GetXPos();
        }

        public void Move(float moveSpeed)
        {
            m_TopPipe.Move(moveSpeed);
            m_BottomPipe.Move(moveSpeed);
            m_ScoreTrigger.transform.position += (Vector3.left * moveSpeed);
        }

        public void SetGap(float gapY, float gapSize)
        {
            float orthoSize = Mathf.Abs(Camera.main.orthographicSize);
            float gapSizeHalf = (gapSize * 0.5f);

            m_TopPipe.SetHeight((orthoSize * 2.0f) - gapY - gapSizeHalf);
            m_BottomPipe.SetHeight(gapY - gapSizeHalf);
        }

        public void DestroySelf()
        {
            m_TopPipe.DestroySelf();
            m_BottomPipe.DestroySelf();
            Destroy(m_ScoreTrigger);
        }
        #endregion
    }

    private class Pipe
    {
        private float CAMERA_ORTHO_SIZE = Mathf.Abs(Camera.main.orthographicSize);

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

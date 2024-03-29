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

    [SerializeField]
    private float TopEdgeLimit;
    [SerializeField]
    private float BottomEdgeLimit;

    private int m_DifficultyIndex;

    private int m_PipesSpawned;
    private float PipeSpawnTimer;

    private float m_SpawnXPos;
    private float m_DestroyXPos;
    private List<GapPipe> m_GapPipesList;

    private EShiftPipesMode m_ShiftPipesMode;

    private bool m_Active;


    private void Awake()
    {
        m_DifficultyIndex = 0;
        m_PipesSpawned = 0;
        PipeSpawnTimer = DifficultyList[m_DifficultyIndex].PipeSpawnRate;

        m_GapPipesList = new List<GapPipe>();

        m_ShiftPipesMode = GameSettings.GetShiftPipesMode();

        m_Active = false;
    }

    private void Start()
    {
        m_SpawnXPos = Mathf.Abs((Camera.main.orthographicSize * 2.0f)) + 5.0f;
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
            float height = GetRandomGapHeight();

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

        if(m_ShiftPipesMode == EShiftPipesMode.ON)
        {
            m_GapPipesList[m_GapPipesList.Count - 1].EnableShift(GetRandomGapHeight());
        }
    }

    private void UpdateDifficulty()
    {
        if (m_DifficultyIndex == DifficultyList.Count - 1) return;  //Reached end of list
        if (m_PipesSpawned >= DifficultyList[m_DifficultyIndex + 1].PipesSpawned) m_DifficultyIndex++;
    }

    private float GetRandomGapHeight()
    {
        //Calculate Min and Max height for the pipes
        float gapSizeHalf = DifficultyList[m_DifficultyIndex].GapSize * 0.5f;
        float minHeight = gapSizeHalf + BottomEdgeLimit;
        float totalHeight = Mathf.Abs((Camera.main.orthographicSize * 2.0f));
        float maxHeight = totalHeight - gapSizeHalf - TopEdgeLimit;

        //Spawn Pipe
        float height = Random.Range(minHeight, maxHeight);
        return height;
    }

    #region Public Methods
    public void Enable(bool enable = true)
    {
        m_Active = enable;
        if (enable) PipeSpawnTimer = -1;    //Spawn a pipe 
    }
    #endregion




    private class GapPipe
    {
        private Pipe m_TopPipe;
        private Pipe m_BottomPipe;
        private GameObject m_ScoreTrigger;

        private float m_GapY;
        private float m_NewGapY;    //For Shifting
        private float m_GapSize;

        private bool m_Shift;
        private float m_ShiftTimer;
        private bool m_Shifting;

        #region Constructor
        public GapPipe(float gapY, float gapSize, float xPos)
        {
            m_GapY = gapY;
            m_GapSize = gapSize;
            m_Shift = false;
            m_Shifting = false;

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

            if(m_Shift && !m_Shifting)
            {
                //Get Screen Position
                Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(m_TopPipe.GetXPos(), 0, 0));

                //Check if Gap Pipe is in view of player's screen
                if (screenPos.x < Screen.width)
                {
                    m_Shift = false;
                    m_Shifting = true;
                }
            }

            if(m_Shifting)
            {
                //LERP between old and new gap y
                m_ShiftTimer += Time.deltaTime;
                float newgap = Mathf.Lerp(m_GapY, m_NewGapY, m_ShiftTimer);
                SetGap(newgap, m_GapSize);

                if(m_ShiftTimer > 1f)
                {
                    //Ensure new gap height is correct
                    SetGap(m_NewGapY, m_GapSize);
                    m_Shifting = false;
                }
            }
        }

        public void SetGap(float gapY, float gapSize)
        {
            float orthoSize = Mathf.Abs(Camera.main.orthographicSize);
            float gapSizeHalf = (gapSize * 0.5f);

            m_TopPipe.SetHeight((orthoSize * 2.0f) - gapY - gapSizeHalf);
            m_BottomPipe.SetHeight(gapY - gapSizeHalf);
        }

        public void EnableShift(float newGapY)
        {
            m_Shift = true;
            m_NewGapY = newGapY;
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

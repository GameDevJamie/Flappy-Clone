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

    private bool m_Active;


    private void Awake()
    {
        m_DifficultyIndex = 0;
        m_PipesSpawned = 0;
        PipeSpawnTimer = DifficultyList[m_DifficultyIndex].PipeSpawnRate;

        m_Active = false;
    }


    // Update is called once per frame
    private void Update()
    {
        if (!m_Active) return;
        HandlePipeSpawn();
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
            float totalHeight = GameAssets.GetInstance().CameraOrthoSize * 2.0f;
            float maxHeight = totalHeight - gapSizeHalf - heightEdgeLimit;

            //Spawn Pipe
            float height = Random.Range(minHeight, maxHeight);
            var pipes = Instantiate(GameAssets.GetInstance().pfGapPipes, Vector3.zero, Quaternion.identity);
            pipes.GetComponent<GapPipeScript>().SetGapSize(height, DifficultyList[m_DifficultyIndex].GapSize);
            m_PipesSpawned++;


            UpdateDifficulty(); //Update Difficulty now to get new spawn rate
            PipeSpawnTimer = DifficultyList[m_DifficultyIndex].PipeSpawnRate;
        }
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
}

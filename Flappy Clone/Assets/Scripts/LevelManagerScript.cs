using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public float MoveSpeed; //Howe fast to move the level (Will override move speed of prefabs)

    public GameObject PipeSpawner;
    public GameObject GroundManager;
    public GameObject CloudManager;

    private PipeSpawnerScript m_PipeSpawnerScript;
    private GroundScript m_GroundManagerScript;
    private CloudScript m_CloudManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        m_PipeSpawnerScript = Instantiate(PipeSpawner, Vector3.zero, Quaternion.identity).GetComponent<PipeSpawnerScript>();
        m_GroundManagerScript = Instantiate(GroundManager, Vector3.zero, Quaternion.identity).GetComponent<GroundScript>();
        m_CloudManagerScript = Instantiate(CloudManager, Vector3.zero, Quaternion.identity).GetComponent<CloudScript>();

        m_PipeSpawnerScript.MoveSpeed = MoveSpeed;
        m_GroundManagerScript.MoveSpeed = MoveSpeed;
        m_CloudManagerScript.MoveSpeed = MoveSpeed * 0.5f;

        //Subscribe to Events
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnStartedPlaying += StartPlaying;
        GameObject.FindGameObjectWithTag("Player").GetComponent<BirdScript>().OnDied += GameOver;

        StopLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StopLevel()
    {
        m_PipeSpawnerScript.Enable(false);
        m_CloudManagerScript.Enable(false);
        m_GroundManagerScript.Enable(false);
    }

    #region Events
    private void StartPlaying(object sender, System.EventArgs a)
    {
        //Tell Managers to start
        m_PipeSpawnerScript.Enable();
        m_CloudManagerScript.Enable();
        m_GroundManagerScript.Enable();
    }

    private void GameOver(object sender, System.EventArgs a)
    {
        StopLevel();
    }
    #endregion
}

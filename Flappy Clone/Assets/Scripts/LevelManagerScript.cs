using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public float MoveSpeed; //Howe fast to move the level (Will override move speed of prefabs)

    //Managers
    public GameObject PipeSpawner;
    public GameObject GroundManager;
    public GameObject CloudManager;

    //Player
    public GameObject Bird;

    //UI
    [SerializeField]
    private GameObject CharacterSelectWindow;
    [SerializeField]
    private GameOverWindow GameOverWindow;

    private PipeSpawnerScript m_PipeSpawnerScript;
    private GroundScript m_GroundManagerScript;
    private CloudScript m_CloudManagerScript;

    private enum EState { CHARACTER_SELECT, PLAYING, GAME_OVER }
    private EState m_State;

    // Start is called before the first frame update
    void Start()
    {
        m_PipeSpawnerScript = Instantiate(PipeSpawner, Vector3.zero, Quaternion.identity).GetComponent<PipeSpawnerScript>();
        m_GroundManagerScript = Instantiate(GroundManager, Vector3.zero, Quaternion.identity).GetComponent<GroundScript>();
        m_CloudManagerScript = Instantiate(CloudManager, Vector3.zero, Quaternion.identity).GetComponent<CloudScript>();

        m_PipeSpawnerScript.transform.SetParent(this.transform);
        m_GroundManagerScript.transform.SetParent(this.transform);
        m_CloudManagerScript.transform.SetParent(this.transform);

        m_PipeSpawnerScript.MoveSpeed = MoveSpeed;
        m_GroundManagerScript.MoveSpeed = MoveSpeed;
        m_CloudManagerScript.MoveSpeed = MoveSpeed * 0.5f;

        //Create Bird object
        Vector3 birdPos = Vector3.zero;
        Bird.transform.position = birdPos;

        //Subscribe to Events
        Bird.GetComponent<BirdScript>().OnDied += GameOver;

        m_State = EState.CHARACTER_SELECT;

        StopLevel();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EState.CHARACTER_SELECT:
                //Has game started
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    StartGame();
                }
                break;

            case EState.PLAYING:
                break;

            case EState.GAME_OVER:
                break;
        }
    }

    private void StopLevel()
    {
        m_PipeSpawnerScript.Enable(false);
        m_CloudManagerScript.Enable(false);
        m_GroundManagerScript.Enable(false);
    }

    private void StartGame()
    {
        //Tell Managers to start
        m_PipeSpawnerScript.Enable();
        m_CloudManagerScript.Enable();
        m_GroundManagerScript.Enable();

        //Hide UI Elements
        CharacterSelectWindow.SetActive(false);

        //Start Bird Jumping
        Bird.GetComponent<BirdScript>().StartJumping();

        m_State = EState.PLAYING;
    }

    #region Events
    private void GameOver(object sender, System.EventArgs a)
    {
        m_State = EState.GAME_OVER;
        StopLevel();

        //Score.TrySetNewHighScore(Bird.GetComponent<BirdScript>().GetPipesPassed());

        GameOverWindow.Show();
    }
    #endregion
}

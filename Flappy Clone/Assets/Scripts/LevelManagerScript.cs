using UnityEngine;

public class LevelManagerScript : MonoBehaviour
{
    public float MoveSpeed; //Howe fast to move the level (Will override move speed of prefabs)

    //Managers
    [Header("Managers")]
    [SerializeField]
    private PipeSpawnerScript PipeSpawner;
    [SerializeField]
    private GroundScript GroundManager;
    [SerializeField]
    private CloudScript CloudManager;

    //Player
    [Header("Player")]
    public GameObject Bird;

    //UI
    [Header("UI")]
    [SerializeField]
    private ToggleActive GameOverWindow;
    [SerializeField]
    private ToggleActive GetReadyWindow;

    [Header("Player Death")]
    [SerializeField]
    private GameObject FlashScreen;


    private enum EState { CHARACTER_SELECT, PLAYING, GAME_OVER }
    private EState m_State;

    // Start is called before the first frame update
    void Start()
    {
        PipeSpawner.MoveSpeed = MoveSpeed;
        GroundManager.MoveSpeed = MoveSpeed;
        CloudManager.MoveSpeed = MoveSpeed * 0.5f;

        //Subscribe to Events
        Bird.GetComponent<BirdScript>().OnDied += GameOver;

        m_State = EState.CHARACTER_SELECT;

        StopLevel();
        GroundManager.Enable();
        CloudManager.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State)
        {
            case EState.CHARACTER_SELECT:
                //Has game started
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    StartGame();
                    return;
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
        PipeSpawner.Enable(false);
        CloudManager.Enable(false);
        GroundManager.Enable(false);
    }

    private void StartGame()
    {
        //Tell Managers to start
        PipeSpawner.Enable();
        CloudManager.Enable();
        GroundManager.Enable();

        //Start Bird Jumping
        Bird.GetComponent<BirdScript>().StartJumping();

        GetReadyWindow.Hide();

        m_State = EState.PLAYING;
    }

    #region Events
    private void GameOver(object sender, System.EventArgs a)
    {
        m_State = EState.GAME_OVER;
        StopLevel();

        //Score.TrySetNewHighScore(Bird.GetComponent<BirdScript>().GetPipesPassed());

        Instantiate(FlashScreen, new Vector3(0, 0, 0), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);

        GameOverWindow.Show();
    }
    #endregion
}

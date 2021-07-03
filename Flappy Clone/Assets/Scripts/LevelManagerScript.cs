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
    private ToggleActive GameOverWindow;
    [SerializeField]
    private ToggleActive GetReadyWindow;
    

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

        //Subscribe to Events
        Bird.GetComponent<BirdScript>().OnDied += GameOver;

        m_State = EState.CHARACTER_SELECT;

        StopLevel();
        m_GroundManagerScript.Enable();
        m_CloudManagerScript.Enable();

        //Spawn all Active Mods
        SpawnMods();
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

        //Start Bird Jumping
        Bird.GetComponent<BirdScript>().StartJumping();

        GetReadyWindow.Hide();

        m_State = EState.PLAYING;
    }

    private void SpawnMods()
    {
        if(ModManager.IsModActive(EModType.WEIGHT))
        {
            GameObject g = new GameObject("Weight_Mod", typeof(WeightMod));
        }
        if (ModManager.IsModActive(EModType.SHIFT))
        {
            //GameObject g = new GameObject("Weight_Mod", typeof(ShiftMod));
        }
        if (ModManager.IsModActive(EModType.MIRROR))
        {
            GameObject g = new GameObject("Mirro_Mod", typeof(MirrorMod));
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();

        transform.Find("RetryBtn").GetComponent<Button>().onClick.AddListener(delegate { levelLoader.StartGame(); } );
        transform.Find("MenuBtn").GetComponent<Button>().onClick.AddListener(delegate { levelLoader.MainMenu(); });
    }
}

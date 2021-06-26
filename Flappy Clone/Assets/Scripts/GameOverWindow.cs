using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("RestartBtn").GetComponent<Button>().onClick.AddListener(RestartLevel);
        Hide();
    }

    private void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }

    #region Public Methods
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    #endregion
}

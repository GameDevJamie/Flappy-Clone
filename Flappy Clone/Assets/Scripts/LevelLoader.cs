using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator Transition;

    [SerializeField]
    private float TransitionTime;

    public void StartGame()
    {
        StartCoroutine(LoadLevel(1));
    }

    public void MainMenu()
    {
        StartCoroutine(LoadLevel(0));
    }

    private IEnumerator LoadLevel(int index)
    {
        Transition.SetTrigger("Start");

        yield return new WaitForSeconds(TransitionTime);

        SceneManager.LoadScene(index);
    }
}

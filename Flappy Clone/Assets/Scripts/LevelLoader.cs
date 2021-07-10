using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup < (start + time))
        {
            yield return null;
        }
    }
}

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private Animator Transition;

    [SerializeField]
    private float TransitionTime;

    private AudioSource AudioSource;

    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
    }

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
        AudioSource.Play();

        yield return CoroutineUtil.WaitForRealSeconds(TransitionTime);

        SceneManager.LoadScene(index);
    }
}

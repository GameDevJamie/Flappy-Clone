using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    [SerializeField]
    private float Timer;

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.unscaledDeltaTime;
        if(Timer < 0f)
        {
            Destroy(this.gameObject);
        }
    }
}

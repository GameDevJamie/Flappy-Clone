using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera _cam;
    private EMirrorMode _mirrorMode;

    // Start is called before the first frame update
    private void Start()
    {
        _cam = GetComponent<Camera>();
        _mirrorMode = GameSettings.GetMirrorMode();
    }

    private void OnPreCull()
    {
        //Do Nothing
        if (_mirrorMode == EMirrorMode.OFF) return;

        _cam.ResetWorldToCameraMatrix();
        _cam.ResetProjectionMatrix();

        Vector3 vec = Vector3.one;
        if (_mirrorMode == EMirrorMode.HORIZONTAL) vec = new Vector3(-1, 1, 1);
        else if (_mirrorMode == EMirrorMode.VERTICAL) vec = new Vector3(1, -1, 1);
        else if (_mirrorMode == EMirrorMode.BOTH) vec = new Vector3(-1, -1, 1);

        _cam.projectionMatrix = _cam.projectionMatrix * Matrix4x4.Scale(vec);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempInput : MonoBehaviour {

    public Camera Cam;

    private void Update()
    {
        Vector3 pos = Input.mousePosition;
        pos = Cam.ScreenToWorldPoint(pos);
        pos.z = -10f;
        transform.position = pos;
    }
}

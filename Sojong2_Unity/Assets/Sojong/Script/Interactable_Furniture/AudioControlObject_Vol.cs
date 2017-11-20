using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControlObject_Vol : AudioControlObject {

    public int Vol;

    public override void OnClick()
    {
        base.OnClick();
        Audio.ChangeVol(Vol);
    }
}

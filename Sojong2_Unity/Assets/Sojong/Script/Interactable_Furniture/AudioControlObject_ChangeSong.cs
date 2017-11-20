using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControlObject_ChangeSong : AudioControlObject {

    public int Change;
    public override void OnClick()
    {
        base.OnClick();
        Audio.ChangeAudio(Change);
    }

}

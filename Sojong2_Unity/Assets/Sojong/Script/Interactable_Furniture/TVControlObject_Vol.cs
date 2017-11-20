using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVControlObject_Vol : TVControlObject {

    public int AddVol;

    public override void OnClick()
    {
        base.OnClick();
        TV.AddVolume(AddVol);
    }
}

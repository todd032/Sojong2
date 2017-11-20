using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVControlObject_Ch : TVControlObject {

    public int ChAdd;

    public override void OnClick()
    {
        base.OnClick();
        TV.ChangeChanel(ChAdd);
    }
}

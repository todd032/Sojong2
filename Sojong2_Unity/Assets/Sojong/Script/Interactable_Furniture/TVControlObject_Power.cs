using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVControlObject_Power : TVControlObject {

    public GameObject OnObject;
    public GameObject OffObject;

    public override void OnClick()
    {
        TV.PowerOn(!TV.Power);
        base.OnClick();

        OnObject.gameObject.SetActive(TV.Power);
        OffObject.gameObject.SetActive(!TV.Power);
    }
}

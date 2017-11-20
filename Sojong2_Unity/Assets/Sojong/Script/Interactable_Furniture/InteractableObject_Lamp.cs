using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_Lamp : InteractableObject {

    public bool On = false;
    public GameObject OnObject;
    public GameObject OffObject;
    public GameObject Light;
    public override void OnClick()
    {
        On = !On;
        base.OnClick();
        OnObject.gameObject.SetActive(On);
        OffObject.gameObject.SetActive(!On);
        Light.gameObject.SetActive(On);
    }
}

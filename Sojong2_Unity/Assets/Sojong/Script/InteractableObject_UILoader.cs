using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject_UILoader : InteractableObject {

    public int UIGroupID;

    public override void OnClick()
    {
        base.OnClick();
        UIManager.Instance.ShowUIGroup(UIGroupID);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject_TestObject : InteractableObject {

    public Image MyImage;

    public bool IsCenterObject;
    public bool IsTarget;

    public void Init(bool _iscenter, bool _istarget)
    {
        IsCenterObject = _iscenter;
        IsTarget = _istarget;
        if (IsTarget)
        {
            MyImage.color = Color.green;
        }else
        {
            MyImage.color = Color.white;
        }
    }

    public override void SetFocus(bool _flag)
    {
        base.SetFocus(_flag);
        if(!IsCenterObject && !IsTarget && _flag)
        {
            //false input.
            AccuracyTestingApp.Instance.InputFail();
        }
       
        if(!IsCenterObject && IsTarget && _flag)
        {
            //clear
            AccuracyTestingApp.Instance.StageClear();
        }
    }
}

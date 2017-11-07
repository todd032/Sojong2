using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeControlManager : MonoBehaviour{


    private static HomeControlManager instance;
    public static HomeControlManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(HomeControlManager)) as HomeControlManager;
            }
            return instance;
        }
    }

    private void Awkae()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public List<SpaceObject> Spaces = new List<SpaceObject>();

    private void Start()
    {

        foreach(SpaceObject obj in Spaces)
        {
            Space newspace = new Space();
            newspace.LeftDownPos = obj.LeftBotPos;
            newspace.With = obj.Width;
            newspace.Height = obj.Height;
            newspace.SpaceType = obj.SpaceType;
            SpaceManager.Instance.AddSpace(newspace);
        }


        SpaceManager.Instance.UpdateFreeSpace();
    }

    public InteractableObject FocusedObject;

    protected float AutoUIDisappearTime = 10f;
    protected float AutoUIDisappearTimer = 0f;
    private void Update()
    {
        if(FocusedObject == null)
        {
            if(InteractableManager.Instance.ActiveObjectList.Count != 0)
            {
                InteractableManager.Instance.UpdateObjectsLink();
                FocusedObject = InteractableManager.Instance.ActiveObjectList[0];
                FocusedObject.SetFocus(true);
                InteractableManager.Instance.ShowCurrentSelectedUI(FocusedObject);
            }
        }
        if(AutoUIDisappearTimer > 0f) {
            AutoUIDisappearTimer -= Time.deltaTime;
            if(AutoUIDisappearTimer < 0f) {
                UIManager.Instance.HideAll();
                //check if focused object is inactive
                if(!FocusedObject.gameObject.active) {
                    FocusedObject.SetFocus(false);
                    FocusedObject = null;
                    if (InteractableManager.Instance.ActiveObjectList.Count != 0) {
                        InteractableManager.Instance.UpdateObjectsLink();
                        FocusedObject = InteractableManager.Instance.ActiveObjectList[0];
                        FocusedObject.SetFocus(true);
                        InteractableManager.Instance.ShowCurrentSelectedUI(FocusedObject);
                    }
                }
            }            
        }
    }

    public void UIChangeEvent()
    {
        InteractableManager.Instance.UpdateObjectsLink();
        InteractableManager.Instance.ShowCurrentSelectedUI(FocusedObject);
    }

    public void ChangeFocus(InteractableObject _obj)
    {
        //Debug.Log("Change focus called");
        if (FocusedObject != null)
        {
            FocusedObject.SetFocus(false);
        }
        FocusedObject = _obj;
        FocusedObject.SetFocus(true);
        InteractableManager.Instance.ShowCurrentSelectedUI(FocusedObject);
    }

    public void InputEvent_Click()
    {
        //Debug.Log("Click Event");
        if (FocusedObject != null)
        {
            FocusedObject.OnClick();
            AutoUIDisappearTimer = AutoUIDisappearTime;
        }
    }

    public void InputEvent_Move(Vector2 _dir)
    {
        Debug.Log("Move Event Called DIR: " + _dir + " ANGLE: " + Constant.GetAngle(Vector2.right, _dir));
        if (FocusedObject != null)
        {
            FocusedObject.ChangeToLinkedObject(_dir);
            AutoUIDisappearTimer = AutoUIDisappearTime;
        }
    }
    
}

public enum HomeControlState
{
    OBJECT_SELECT,
    BUTTON_SELECT,
}

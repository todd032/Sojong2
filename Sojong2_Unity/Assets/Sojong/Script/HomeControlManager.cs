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
    private void Update()
    {
        if(FocusedObject == null)
        {
            if(InteractableManager.Instance.ActiveObjectList.Count != 0)
            {
                InteractableManager.Instance.UpdateObjectsLink();
                FocusedObject = InteractableManager.Instance.ActiveObjectList[0];
                FocusedObject.Focused = true;
                InteractableManager.Instance.ShowCurrentSelectedUI(FocusedObject);
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
        FocusedObject.Focused = false;
        FocusedObject = _obj;
        InteractableManager.Instance.ShowCurrentSelectedUI(FocusedObject);
    }

    public void InputEvent_Click()
    {
        Debug.Log("Click Event");
        FocusedObject.OnClick();
    }

    public void InputEvent_Move(Vector2 _dir)
    {
        Debug.Log("Move Event Called: " + _dir);
        FocusedObject.ChangeToLinkedObject(_dir);
    }
}

public enum HomeControlState
{
    OBJECT_SELECT,
    BUTTON_SELECT,
}

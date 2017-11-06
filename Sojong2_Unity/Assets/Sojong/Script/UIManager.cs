using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {


    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null) {
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            }
            return instance;
        }
    }


    public List<UIGroup> UIGroupList = new List<UIGroup>();

    public void AddUIGroup(UIGroup _uigroup)
    {
        UIGroupList.Add(_uigroup);
    }

    public void HideAll()
    {
        for (int iter = 0; iter < UIGroupList.Count; iter++) {
            UIGroupList[iter].HideUI();
        }
        HomeControlManager.Instance.UIChangeEvent();
    }

    public void HideUIGroup(int _id)
    {
        for(int iter = 0; iter < UIGroupList.Count; iter++) {
            UIGroupList[iter].HideUI();
        }
        HomeControlManager.Instance.UIChangeEvent();
    }

    public void ShowUIGroup(int _id)
    {
        for (int iter = 0; iter < UIGroupList.Count; iter++) {
            UIGroup curgroup = UIGroupList[iter];
            if (_id == curgroup.UIGroupId)
                curgroup.ShowUI();
            else
                curgroup.HideUI();
        }
        HomeControlManager.Instance.UIChangeEvent();
    }
}

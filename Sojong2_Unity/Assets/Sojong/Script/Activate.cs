using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour {

    public void ActivateF()
    {
        gameObject.SetActive(true);

        HomeControlManager.Instance.UIChangeEvent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableObject_InputDisplayer : MonoBehaviour {

    public float Threshold = 0.2f;
    public float Radius = 100f;

    public int TrackerCount = 10;
    public Transform Mat;
    public List<Transform> TrackerList = new List<Transform>();

    private void Awake()
    {
        for(int iter = 0; iter < TrackerCount; iter++)
        {
            TrackerList.Add(Instantiate(Mat.gameObject, Mat.parent).transform);
            
        }
        TrackerList[TrackerList.Count - 1].GetComponent<Image>().color = Color.yellow;
    }
    

    void Update()
    {
        //Debug.Log("??");
        for(int iter = 0; iter < TrackerList.Count ; iter++)
        {
            if (iter < TrackerList.Count - 1)
            {
                TrackerList[iter].localPosition = TrackerList[iter + 1].localPosition;
            }
            else
            {
                //Debug.Log("Set to:  InputManager.Instance.CurVelocity / Threshold * Radius;");
                TrackerList[iter].localPosition = InputManager.Instance.CurVelocity / Threshold * Radius;
            }
        }
        //transform.localPosition = InputManager.Instance.CurVelocity / Threshold * Radius;
    }

}

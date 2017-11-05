using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

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

    public int InputDataMax = 5;
    public List<InputData> InputDataList = new List<InputData>();
    public float MoveEventThreshold = 100f;
    protected float MoveEventDelayTimer = 0f;
    public float MoveEventDelayTime = 3f;
    private void Update()
    {
        InputData newdata = new InputData();
        newdata.AbsolutePos = Input.mousePosition;

        InputDataList.Add(newdata);
        if(InputDataList.Count > InputDataMax)
        {
            InputDataList.RemoveAt(0);
        }
        
        if(InputDataList.Count > 1)
        {
            newdata.DeltaDir = newdata.AbsolutePos - InputDataList[InputDataList.Count - 2].AbsolutePos;
            newdata.AccumDeltaDir = newdata.AbsolutePos - InputDataList[0].AbsolutePos;
        }

        if(Input.GetMouseButtonDown(0))
        {
            HomeControlManager.Instance.InputEvent_Click();
        }else
        {
            if (MoveEventDelayTimer > 0f)
            {
                MoveEventDelayTimer -= Time.deltaTime;
            }
            else
            {
                if (newdata.AccumDeltaDir.magnitude > MoveEventThreshold)
                {
                    HomeControlManager.Instance.InputEvent_Move(newdata.AccumDeltaDir);
                    MoveEventDelayTimer = MoveEventDelayTime;
                }
            }
        }
    }
}

public class InputData
{
    public Vector2 AbsolutePos;
    public Vector2 DeltaDir;
    public Vector2 AccumDeltaDir;
}


public class InputEvent
{
    public InputEventType _type;
    public Vector2 Direction;
}

public enum InputEventType
{
    Click,
    Move,
}


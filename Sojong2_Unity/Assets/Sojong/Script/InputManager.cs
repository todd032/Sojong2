using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private static InputManager instance;
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(InputManager)) as InputManager;
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        //UpdateInputByTimeLimit();
        //UpdateInputByVelocity();
        UpdateNetworkInput();
    }

    public int InputDataMax = 5;
    public List<InputData> InputDataList = new List<InputData>();
    public float MoveEventThreshold = 100f;
    protected float MoveEventDelayTimer = 0f;
    public float MoveEventDelayTime = 3f;
    protected void UpdateInputByTimeLimit()
    {
        InputData newdata = new InputData();
        newdata.AbsolutePos = Input.mousePosition;

        InputDataList.Add(newdata);
        if (InputDataList.Count > InputDataMax)
        {
            InputDataList.RemoveAt(0);
        }

        if (InputDataList.Count > 1)
        {
            newdata.DeltaDir = newdata.AbsolutePos - InputDataList[InputDataList.Count - 2].AbsolutePos;
            newdata.AccumDeltaDir = newdata.AbsolutePos - InputDataList[0].AbsolutePos;
        }

        if (Input.GetMouseButtonDown(0))
        {
            HomeControlManager.Instance.InputEvent_Click();
        }
        else
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

    public float VelocityReducerByFrame = 0.99f;
    public float VelocityReducerByEvent = 0.5f;
    public Vector2 CurVelocity;
    public float VelocityThreshold = 50f;
    protected InputData PrevInputData;
    protected void UpdateInputByVelocity()
    {
        CurVelocity *= VelocityReducerByFrame;

        InputData newdata = new InputData();
        newdata.AbsolutePos = Input.mousePosition;
        if(PrevInputData != null)
        {
            CurVelocity += (newdata.AbsolutePos - PrevInputData.AbsolutePos);
        }

        if (Input.GetMouseButtonDown(0))
        {
            HomeControlManager.Instance.InputEvent_Click();
        }
        else
        {
            if (MoveEventDelayTimer > 0f)
            {
                MoveEventDelayTimer -= Time.deltaTime;
            }
            else
            {
                if (CurVelocity.magnitude > VelocityThreshold)
                {
                    MoveEventDelayTimer = 0.25f;
                    HomeControlManager.Instance.InputEvent_Move(CurVelocity);
                    CurVelocity *= VelocityReducerByEvent;
                }
            }
        }

        PrevInputData = newdata;
    }

    public float MaxChange = 0.1f;
    protected NetworkMessage PrevMessage;
    protected List<NetworkMessage> ProcessMessageList = new List<NetworkMessage>();
    protected Queue<NetworkMessage> MessageQueue = new Queue<NetworkMessage>();
    public void Addinput(NetworkMessage _message)
    {
        MessageQueue.Enqueue(_message);
    }

    protected void UpdateNetworkInput()
    {
        ProcessMessageList.Clear();
        int messagecount = MessageQueue.Count;
        for (int iter = 0; iter < messagecount; iter++)
        {
            ProcessMessageList.Add(MessageQueue.Dequeue());
        }

        if (messagecount > 0)
        {
            CurVelocity *= VelocityReducerByFrame;
        }

        bool clickedinloop = false;
        for (int iter = 0; iter < ProcessMessageList.Count; iter++)
        {
            NetworkMessage _message = ProcessMessageList[iter];


            //InputData newdata = new InputData();
            //newdata.AbsolutePos = _message.;
            //if (PrevInputData != null)
            //{
            //    CurVelocity += (newdata.AbsolutePos - PrevInputData.AbsolutePos);
            //}
            Vector2 input = new Vector2(_message.X, _message.Y);
            if(input.magnitude > MaxChange)
            {
                input = input.normalized * MaxChange;
                Debug.Log("Input max change caught");
            }
            CurVelocity += input;

            //if (Input.GetMouseButtonDown(0))
            if (!clickedinloop && PrevMessage != null && !PrevMessage.isClicked && _message.isClicked)
            {
                clickedinloop = true;
                HomeControlManager.Instance.InputEvent_Click();
            }
            else
            {
                if (MoveEventDelayTimer > 0f)
                {
                    MoveEventDelayTimer -= Time.deltaTime;
                }
                else
                {
                    if (CurVelocity.magnitude > VelocityThreshold)
                    {
                        MoveEventDelayTimer = MoveEventDelayTime;
                        HomeControlManager.Instance.InputEvent_Move(CurVelocity);
                        CurVelocity *= VelocityReducerByEvent;
                        Debug.Log("Move called: " + CurVelocity);
                    }
                }
            }

            // PrevInputData = newdata;
            PrevMessage = _message;
        }
    }
    
private void OnDrawGizmosSelected()
    {
        if (HomeControlManager.Instance.FocusedObject != null)
        {
            Vector3 dir = CurVelocity.normalized;
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(HomeControlManager.Instance.FocusedObject.transform.position, dir * VelocityThreshold);

            Gizmos.color = Color.red;            
            Gizmos.DrawRay(HomeControlManager.Instance.FocusedObject.transform.position, CurVelocity);
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


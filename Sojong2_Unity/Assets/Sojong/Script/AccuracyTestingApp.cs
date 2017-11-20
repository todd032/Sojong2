using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

using UnityEngine;

public class AccuracyTestingApp : MonoBehaviour {
        
    private static AccuracyTestingApp instance;
    public static AccuracyTestingApp Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(AccuracyTestingApp)) as AccuracyTestingApp;
            }
            return instance;
        }
    }
    
    private void Start()
    {
        CreateStage();
    }

    private void Update()
    {
        ClearTimer += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            SaveTestData();
        }
    }

    //public string SaveFileName = "Result.txt";
    public List<TestData> TestDataList = new List<TestData>();
    public void SaveTestData()
    {
        string path = Application.dataPath + "/";
        string csv = "";
        
        if(!File.Exists(path + "Case1_Result.txt"))
        {
            File.WriteAllText(path + "Case1_Result.csv", " Case 1, Success, Response");
        }

        if (!File.Exists(path + "Case2_Result.txt"))
        {
            File.WriteAllText(path + "Case2_Result.csv", " Case 2, Success, Response");
        }

        if (!File.Exists(path + "Case3_Result.txt"))
        {
            File.WriteAllText(path + "Case3_Result.csv", " Case 3, Success, Response");
        }

        if (!File.Exists(path + "Case4_Result.txt"))
        {
            File.WriteAllText(path + "Case4_Result.csv", " Case 4, Success, Response");
        }

        for(int iter = 0; iter < TestDataList.Count; iter++)
        {
            TestData curdata = TestDataList[iter];
            string savefile = path + "Case" + (curdata.Case + 1).ToString() + "_Result.csv";
            string data = "\n," + (curdata.Success ? 1 : 0) + "," + curdata.ResponseTime.ToString() + "";
            File.AppendAllText(savefile, data);
        }
        TestDataList.Clear();

        Debug.Log("Data Saved");
    }
    
    public int TestPerCase = 10;
    public int Stage = 0;
    public float ClearTimer = 0;
    public void CreateStage()
    {
        ClearTimer = 0f;
        InputManager.Instance.CurVelocity = Vector2.zero;
        if(Stage < 1 * TestPerCase)
        {
            CreateButtons(90f, 1f);
        }else if (Stage < 2 * TestPerCase)
        {
            CreateButtons(60f, 1f);
        }else if(Stage < 3 * TestPerCase)
        {
            CreateButtons(45f, 1f);
        }else if(Stage < 4 * TestPerCase)
        {
            CreateButtons(30f, 1f);
        }
        
        HomeControlManager.Instance.ChangeFocus(TestObjectList[0]);
        HomeControlManager.Instance.UIChangeEvent();
        Stage = (Stage + 1) % 40;
    }
    

    public Transform ButtonPivot;
    public Object ButtonPrefab;
    public List<InteractableObject_TestObject> TestObjectList = new List<InteractableObject_TestObject>();
    public void CreateButtons(float _angle, float _createratio, bool _randstartangle = false)
    {
        //inactivate all objects
        for(int iter = 0; iter < TestObjectList.Count; iter++)
        {
            TestObjectList[iter].gameObject.SetActive(false);
        }

        //set center
        InteractableObject_TestObject centerobj = null;
        if(TestObjectList.Count == 0)
        {
            CreateNewButtonObject();
        }
        centerobj = TestObjectList[0];
        centerobj.gameObject.SetActive(true);
        centerobj.transform.localPosition = Vector3.zero;
        centerobj.Init(true, false);

        //set targets        
        int index = 0;
        float angleaccum = 0f;
        float randstartangle = 0f;
        if (_randstartangle)
        {
            randstartangle = Random.Range(0f, 360f); 
            angleaccum += randstartangle;
        }
        while(angleaccum < 360f + randstartangle)
        {           
            if (Random.RandomRange(0f, 1f) < _createratio)
            {
                index++;
                float rad = angleaccum * Mathf.Deg2Rad;
                Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
                float radius = 200f;
                if (TestObjectList.Count <= index)
                {
                    CreateNewButtonObject();
                }
                InteractableObject_TestObject curobj = TestObjectList[index];
                curobj.gameObject.SetActive(true);
                curobj.Init(false, false);
                curobj.transform.localPosition = dir * radius;
            }
            angleaccum += _angle;
        }

        //set random target
        TestObjectList[Random.RandomRange(1, index + 1)].Init(false, true);
    }

    protected void  CreateNewButtonObject()
    {
        TestObjectList.Add((Instantiate(ButtonPrefab,ButtonPivot) as GameObject).GetComponent<InteractableObject_TestObject>());
    }

    public void StageClear()
    {
        
        StartCoroutine(StageCompleteAnim(true));
    }

    public void InputMissEvent()
    {
        Debug.Log("Input miss");
    }

    public void InputFail()
    {
        //CreateStage();
        StartCoroutine(StageCompleteAnim(false));
    }

    public GameObject Success;
    public GameObject Fail;

    protected IEnumerator StageCompleteAnim(bool _success)
    {
        TestData newtestdata = new TestData();
        newtestdata.Case = (Stage - 1) / TestPerCase;
        newtestdata.Success = _success; ;
        newtestdata.ResponseTime = ClearTimer;
        TestDataList.Add(newtestdata);

        InputManager.Instance.gameObject.SetActive(false);
        if(_success)
        {
            Success.gameObject.SetActive(true);
        }else
        {
            Fail.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.5f);
        
        Success.gameObject.SetActive(false);
        Fail.gameObject.SetActive(false);
        InputManager.Instance.gameObject.SetActive(true);
        CreateStage();
        yield return null;
    }
}

public class TestData
{
    public int Case;
    public bool Success;
    public float ResponseTime;
}

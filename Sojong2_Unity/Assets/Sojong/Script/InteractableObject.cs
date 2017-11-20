using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public bool Focused = false;
    public bool Shrink = false;
    public float ShrinkFactor = 0.8f;
    protected Vector3 StartSize;
    public GameObject FocusedObject;
    
    private void Awake()
    {
        StartSize = transform.localScale;
        InteractableManager.Instance.AddToList(this);
        if(gameObject.active )
        {
            InteractableManager.Instance.SetToActive(this);
        }
    }
    
    // Use this for initialization
    void Start () {
        
        SetFocus(Focused);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public virtual void SetFocus(bool _flag)
    {
        Focused = _flag;
        FocusedObject.SetActive(Focused);
        if (Focused)
        {
            FocusedObject.GetComponent<ParticleSystem>().Play();
        }

        if (Shrink)
        {
            if (!Focused)
            {
                transform.localScale = StartSize * ShrinkFactor;
            }
            else
            {
                transform.localScale = StartSize;
            }
        }
    }
    private void OnEnable()
    {
        InteractableManager.Instance.SetToActive(this);
        //Debug.Log("Hello?");
    }

    private void OnDisable()
    {
        if(InteractableManager.Instance != null)
            InteractableManager.Instance.SetToInactive(this);
    }

    public List<InteractableObject> LinkedObject = new List<InteractableObject>();
    public void ClearLinkedObject()
    {
        LinkedObject.Clear();
    }

    public void AddLinkedObject(InteractableObject _obj)
    {
        LinkedObject.Add(_obj);
    }

    public virtual void OnClick()
    {
        StartCoroutine(BumpAnim());
    }

    public void ChangeToLinkedObject(Vector2 _input)
    {
        float targetangle = Constant.GetAngle(Vector2.right, _input.normalized);
        float minangle = 360f;
        int minindex = -1;
        for(int iter = 0; iter < LinkedObject.Count; iter++)
        {
            InteractableObject linkedobj = LinkedObject[iter];

            Vector2 deltadir = linkedobj.transform.position - transform.position;
            float curangle = Vector2.Angle(_input, deltadir);
            float deltaangle = curangle;
            if(deltaangle < minangle && deltaangle < 45f)
            {
                minangle = deltaangle;
                minindex = iter;
            }
        }   

        if(minindex != -1)
        {
            HomeControlManager.Instance.ChangeFocus(LinkedObject[minindex]);
        }
    }

    protected IEnumerator BumpAnim()
    {
        Vector3 startscale = StartSize;

        float timer = 0f;
        float sinval = 0f;
        while(timer < 0.5f)
        {
            timer += Time.deltaTime;
            sinval += Time.deltaTime * 360f;
            transform.localScale = startscale * (1f + 0.2f * Mathf.Sin(sinval * Mathf.Deg2Rad));
            yield return null;
        }

        if(Focused)
        { 
            transform.localScale = StartSize;
        }else
        {
            if(Shrink)
            {
                transform.localScale = StartSize * ShrinkFactor;
            }else
            {
                transform.localScale = StartSize;
            }
        }
        yield return null;
    }
}

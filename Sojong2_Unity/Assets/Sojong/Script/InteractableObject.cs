using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    public bool Focused = false;
    public GameObject FocusedObject;

    private void Awake()
    {
        InteractableManager.Instance.AddToList(this);
        if(gameObject.active )
        {
            InteractableManager.Instance.SetToActive(this);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (FocusedObject.active != Focused)
        {
            FocusedObject.SetActive(Focused);
            if(Focused)
            {
                FocusedObject.GetComponent<ParticleSystem>().Play();
                
            }
        }
    }

    private void OnEnable()
    {
        InteractableManager.Instance.SetToActive(this);
        Debug.Log("Hello?");
    }

    private void OnDisable()
    {
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

    public void OnClick()
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
            float curangle = Constant.GetAngle(Vector2.right, deltadir);
            float deltaangle = Mathf.Abs(targetangle - curangle);
            if(deltaangle < minangle && deltaangle < 90f)
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
        Vector3 startscale = transform.localScale;

        float timer = 0f;
        float sinval = 0f;
        while(timer < 0.5f)
        {
            timer += Time.deltaTime;
            sinval += Time.deltaTime * 360f;
            transform.localScale = startscale * (1f + 0.2f * Mathf.Sin(sinval * Mathf.Deg2Rad));
            yield return null;
        }

        transform.localScale = startscale;
        
        yield return null;
    }
}

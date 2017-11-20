using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableManager : MonoBehaviour {

    private static InteractableManager instance;
    public static InteractableManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType(typeof(InteractableManager)) as InteractableManager;
            }
            return instance;
        }
    }

    private void Awkae()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {

    }

    public List<InteractableObject> ObjectList = new List<InteractableObject>();
    public List<InteractableObject> ActiveObjectList = new List<InteractableObject>();
    public List<InteractableObject> InActiveObjecList = new List<InteractableObject>();

    public void AddToList(InteractableObject _obj)
    {
        if(!ObjectList.Contains(_obj))
        {
            ObjectList.Add(_obj);
        }
    }

    public void SetToInactive(InteractableObject _obj)
    {
        if(ActiveObjectList.Contains(_obj))
        {
            ActiveObjectList.Remove(_obj);
        }
    }

    public void SetToActive(InteractableObject _obj)
    {
        if(!ActiveObjectList.Contains(_obj))
        {
            ActiveObjectList.Add(_obj);
        }
    }

    public void UpdateObjectsLink()
    {
        //
        for(int iter = 0; iter < ActiveObjectList.Count; iter++)
        {
            InteractableObject curobj = ActiveObjectList[iter];
            curobj.ClearLinkedObject();

            List<float> DistList = new List<float>();
            List<InteractableObject> ObjectByDistance = new List<InteractableObject>();
            List<InteractableObject> LinkedList = new List<InteractableObject>();
            List<float> LinkedAngle = new List<float>();
            for(int iter2 = 0; iter2 < ActiveObjectList.Count; iter2++)
            {
                InteractableObject newobj = ActiveObjectList[iter2];
                if(iter == iter2)
                {

                }else
                {
                    float distance = Vector2.Distance(curobj.transform.position, newobj.transform.position);
                    bool added = false;
                    
                    
                    for (int distiter = 0; distiter < ObjectByDistance.Count; distiter++)
                    {
                        float curdist = DistList[distiter];
                        if(distance < curdist)
                        {
                            //Debug.Log("Dist: " + distance + " curdist: " + curdist);
                            added = true;
                            DistList.Insert(distiter, distance);
                            ObjectByDistance.Insert(distiter, newobj);
                            break;
                        }
                    }
                    if(!added)
                    {
                        ObjectByDistance.Add(newobj);
                        DistList.Add(distance);
                    }
                }
            }

            //create link list
            for(int distiter = 0; distiter < ObjectByDistance.Count; distiter++)
            {
                InteractableObject newobj = ObjectByDistance[distiter];

                Vector2 axis = Vector2.right;
                Vector2 dir = newobj.transform.position - curobj.transform.position;

                float angle = Constant.GetAngle(axis, dir);

                //comp if there are no interruption between two objs
                RaycastHit2D[] hits = Physics2D.RaycastAll(curobj.transform.position, dir, DistList[distiter], LayerMask.GetMask(new string[] { "UIObject" }));

                //Debug.Log("Hits count: " + hits.Length);
                bool notfoundanybetween = true;
                foreach(RaycastHit2D hit in hits)
                {
                    if(hit.collider != null)
                    {
                        InteractableObject hitobj = hit.collider.GetComponent<InteractableObject>();
                        if(hitobj != null && hitobj != curobj && hitobj != newobj)
                        {
                            //other object detected.
                            notfoundanybetween = false;
                            break;
                        }
                    }
                }
                if(notfoundanybetween)
                {
                    //add link
                    LinkedList.Add(newobj);
                    LinkedAngle.Add(angle);
                }
            }

            for(int linkediter = 0; linkediter < LinkedList.Count; linkediter++)
            {
                InteractableObject linkedobj = LinkedList[linkediter];
                curobj.AddLinkedObject(linkedobj);
            }
        }
    }

    public Transform LinePivot;
    public Object LinkLinePrefab;
    public List<LinkLineObject> LinkLineList = new List<LinkLineObject>();
    public void ShowLink(InteractableObject _center, List<InteractableObject> _linked)
    {
        for(int iter = 0; iter < LinkLineList.Count; iter++)
        {
            Destroy(LinkLineList[iter].gameObject);
        }
        LinkLineList.Clear();

        for(int iter = 0; iter < _linked.Count; iter++)
        {
            InteractableObject curlink = _linked[iter];

            LinkLineObject newobj = (Instantiate(LinkLinePrefab) as GameObject).GetComponent<LinkLineObject>();

            newobj.SetLine(_center.transform.position, curlink.transform.position);
            newobj.transform.SetParent(LinePivot);

            LinkLineList.Add(newobj);
        }
    }

    public void ShowCurrentSelectedUI()
    {
        //Debug.Log("show current selected ui");
        UpdateObjectsLink();        
        ShowLink(ActiveObjectList[0], ActiveObjectList[0].LinkedObject);
        
    }

    public void ShowCurrentSelectedUI(InteractableObject _obj)
    {
        //Debug.Log("show current selected ui");
        //UpdateObjectsLink();
        ShowLink(_obj, _obj.LinkedObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EasyIKBone))]
public class EasyIKManager : MonoBehaviour {

    public float weight =1f;
    public Transform target;
    public uint chainLength = 1;
    EasyIKBone end;
    EasyIKBone start;
    Vector3 rootPosOffset;
    // Use this for initialization
    private void Awake()
    {
        if (chainLength < 1)
        {
            chainLength = 1;
        }
    }
    // initialize the esay ik bones
    void Start () {
        Transform p = transform.parent;
        EasyIKBone current = gameObject.GetComponent<EasyIKBone>();
        end = current;
        start = current;
		for(int i = 1; i <= chainLength; i++)
        {
            EasyIKBone parent;
            if (chainLength == i)
            {
                if (p != null)
                    parent = p.gameObject.GetComponent<EasyIKBone>();
                else
                    parent = null;
            }else
            {
                if (p != null)
                {
                    parent = p.gameObject.GetComponent<EasyIKBone>();
                    if(parent == null)
                        parent = p.gameObject.AddComponent<EasyIKBone>();
                    start = parent;
                }
                else
                {
                    parent = null;
                }
            }
            current.Init(parent);
            if(p == null)
            {
                break;
            }
            p = p.parent;
            current = parent;
            
        }
        if (start.transform.parent != null)
            rootPosOffset = start.transform.position - start.transform.parent.position;
        else
            rootPosOffset = Vector3.zero;
	}
	

    // calculate the target position and update the bones' rotation according to the position.
	void LateUpdate () {

        end.Follow(target.position);
        end.CalculateEndP();
        EasyIKBone next = end.parent;
        while (next != null)
        {
            next.Follow();
            next.CalculateEndP();
            next = next.parent;
        }
        if (start.transform.parent!=null)
            start.SetStartP(rootPosOffset + start.transform.parent.position);
        else
            start.SetStartP(start.transform.position);

        start.CalculateEndP();
        start.UpdateRotation(weight);
        next = start.child;
        
        while (next != null)
        {
            next.AttachStartP();
            next.CalculateEndP();
            next.UpdateRotation(weight);
            next = next.child;
        }
    }
}

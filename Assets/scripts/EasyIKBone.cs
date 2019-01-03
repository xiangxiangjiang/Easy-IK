using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyIKBone : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Vector3 startP;
    [SerializeField]
    Vector3 endP;
    [SerializeField]
    float boneLength = 0f;
    [SerializeField]

    public EasyIKBone parent=null;
    public EasyIKBone child = null;

    private Vector3 boneAxis = new Vector3(-1.0f, 0.0f, 0.0f);
    private Transform org;
    private Quaternion localRotation;
    //private EasyIKManager managerRef;
    private Vector3 targetDir;
    public void Init(EasyIKBone parent_)
    {
        startP = transform.position;
        localRotation = transform.localRotation;
        //managerRef = manager;
        parent = parent_;
        if (parent != null)
        {
            parent.child = this;
        }

        if (child != null)
        {
            endP = child.transform.position;
            boneLength = Vector3.Distance(startP, endP);
        }
        else
        {
            endP = startP;
            boneLength = 0f;
        }
    }
    public void SetStartP(Vector3 p)
    {
        startP = p;
    }
    public void AttachStartP()
    {
        SetStartP(parent.endP);
    }
    public void Follow()
    {
        
         Follow(child.startP);
        
    }
    public void Follow(Vector3 target)
    {

        targetDir = target - startP;
        targetDir.Normalize();
        Vector3 dir = targetDir;
        dir.Scale(new Vector3(-1,-1,-1)*boneLength);
        startP = target+dir;
    }
    public void CalculateEndP()
    {
        endP = startP + targetDir*boneLength;
    }

    public void UpdateRotation (float weight) {

        // rotate from original direction to target direction.
        org = transform;
        transform.localRotation = Quaternion.identity * localRotation;
        Vector3 aimVector = transform.TransformDirection(boneAxis);
        Quaternion aimRotation = Quaternion.FromToRotation(aimVector, endP-startP);
        Quaternion secondaryRotation = aimRotation * transform.rotation;
        transform.rotation = Quaternion.Lerp(org.rotation, secondaryRotation, weight);
        //transform.position = startP;
    }
}

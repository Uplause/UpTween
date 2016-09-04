using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[System.Serializable]
public class UpTweenTransformValues : UpTweenAbstractValues
{
    public Transform target;

    public bool enable_position = true;
    public bool enable_scale;
    public bool enable_rotation;

    public Vector3 pos;
    public Vector3 scale;
    public Vector3 rotation;

    // Original transformation
    [HideInInspector]
    public Vector3 o_pos;
    [HideInInspector]
    public Vector3 o_rot;
    [HideInInspector]
    public Vector3 o_scale;

    public override void SetToStart()
    {
        if (enable_position)
        {
            parent.target.position = new Vector3(GetPos().x, GetPos().y, GetPos().z);
        }
        if (enable_rotation)
        {
            parent.target.rotation = Quaternion.Euler(GetRot().x, GetRot().y, GetRot().z);
        }
        if (enable_scale)
        {
            parent.target.localScale = new Vector3(GetScale().x, GetScale().y, GetScale().z);
        }
    }

    public override void CopyStart()
    {
        if (!target)
            target = parent.transform;

        if (enable_position)
        {
            pos.x = parent.target.position.x;
            pos.y = parent.target.position.y;
            pos.z = parent.target.position.z;
        }
        if (enable_rotation)
        {
            rotation.x = parent.target.eulerAngles.x;
            rotation.y = parent.target.eulerAngles.y;
            rotation.z = parent.target.eulerAngles.z;
        }
        if (enable_scale)
        {
            scale.x = parent.target.localScale.x;
            scale.y = parent.target.localScale.y;
            scale.z = parent.target.localScale.z;
        }
    }

    public Vector3 GetPos()
    {
        if (target)
            return target.transform.position;
        else
            return pos;
    }

    public Vector3 GetRot()
    {
        if (target)
            return target.transform.rotation.eulerAngles;
        else
            return rotation;
    }

    public Vector3 GetScale()
    {
        if (target)
            return target.transform.localScale;
        else
            return scale;
    }

    public override void SetOriginalPositions()
    {
        o_pos = new Vector3(parent.target.position.x, parent.target.position.y, parent.target.position.z);
        o_rot = new Vector3(parent.target.rotation.eulerAngles.x, parent.target.rotation.eulerAngles.y, parent.target.rotation.eulerAngles.z);
        o_scale = new Vector3(parent.target.localScale.x, parent.target.localScale.y, parent.target.localScale.z);
    }

    public override void Update(UpTween target, UpTweenAbstractValues _A, UpTweenAbstractValues _B, float animation_time)
    {
        UpTweenTransformValues A = (UpTweenTransformValues)_A;
        UpTweenTransformValues B = (UpTweenTransformValues)_B;

        Vector3 origin_pos = new Vector3(0, 0, 0);
        Vector3 origin_rot = Vector3.zero;
        Vector3 origin_scale = Vector3.zero;

        if (target.additive)
        {
            origin_pos = new Vector3(A.o_pos.x, A.o_pos.y, A.o_pos.z);
            origin_rot = new Vector3(A.o_rot.x, A.o_rot.y, A.o_rot.z);
            origin_scale = new Vector3(A.o_scale.x, A.o_scale.y, A.o_scale.z);
        }

        if (A.enable_position)
            A.parent.target.position = origin_pos + A.GetPos() + (B.GetPos() - A.GetPos()) * animation_time;
        if (A.enable_rotation)
            A.parent.target.rotation = Quaternion.Euler(origin_rot + A.GetRot() + (B.GetRot() - A.GetRot()) * animation_time);
        if (A.enable_scale)
            A.parent.target.localScale = origin_scale + A.GetScale() + (B.GetScale() - A.GetScale()) * animation_time;
    }
}
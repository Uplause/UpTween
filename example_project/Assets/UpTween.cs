/*  Copyright (c) 2016 Uplause Ltd
    Original author Jalmari Ikävalko, tzaeru@gmail.com
    
    Licensed under MIT. See the associated LICENSE text file for details.

    Contributors welcome!
*/

using UnityEngine;
using System.Collections;

[System.Serializable]
public class UpTweenTransformation
{
    public GameObject target;
    public Vector3 pos;
    public Vector3 scale;
    public Vector3 rotation;

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
}

public class UpTween : MonoBehaviour {
    public Transform target;

    public bool enable_position = true;
    public bool enable_scale;
    public bool enable_rotation;

    public UpTweenTransformation start;
    public UpTweenTransformation end;

    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    
    public float duration = 1.0f;

    public bool additive = true;

    public enum LOOP { NONE = 0, ONCE = 1, FOREVER = 2}
    public LOOP loop = LOOP.NONE;
    public bool pingpong = true;

    public bool play_at_start = false;

    [InspectorButton("CopyStart")]
    public bool copy_start;

    [InspectorButton("CopyEnd")]
    public bool copy_end;

    [InspectorButton("SetToStart")]
    public bool set_to_start;

    [InspectorButton("SetToEnd")]
    public bool set_to_end;

    [InspectorButton("PlayFromStart")]
    public bool start_tween;

    [InspectorButton("Stop")]
    public bool stop_tween;

    [InspectorButton("Continue")]
    public bool continue_tween;

    // Original transformation
    Vector3 o_pos;
    Vector3 o_rot;
    Vector3 o_scale;

    bool active = false;
    int direction = -1;
    float time = 0.0f;
    int loop_times = 0;

    public void PlayFromStart()
    {
        Play(true);
    }

    public void Play(bool reset_loop_times = true)
    {
        active = true;
        if (direction == -1)
            direction = 1;
        else
            direction = -1;

        if (!pingpong)
            direction = 1;

        if (reset_loop_times)
            loop_times = 0;

        time = 0.0f;
    }

    public void Stop()
    {
        active = false;
    }

    public void Continue()
    {
        active = true;
    }

    public void CopyStart()
    {
        if (!target)
            target = transform;

        if (enable_position)
        {
            start.pos.x = target.position.x;
            start.pos.y = target.position.y;
            start.pos.z = target.position.z;
        }
        if (enable_rotation)
        {
            start.rotation.x = target.eulerAngles.x;
            start.rotation.y = target.eulerAngles.y;
            start.rotation.z = target.eulerAngles.z;
        }
        if (enable_scale)
        {
            start.scale.x = target.localScale.x;
            start.scale.y = target.localScale.y;
            start.scale.z = target.localScale.z;
        }
    }

    public void CopyEnd()
    {
        if (!target)
            target = transform;

        if (enable_position)
        {
            end.pos.x = target.position.x;
            end.pos.y = target.position.y;
            end.pos.z = target.position.z;
        }
        if (enable_rotation)
        {
            end.rotation.x = target.eulerAngles.x;
            end.rotation.y = target.eulerAngles.y;
            end.rotation.z = target.eulerAngles.z;
        }
        if (enable_scale)
        {
            end.scale.x = target.localScale.x;
            end.scale.y = target.localScale.y;
            end.scale.z = target.localScale.z;
        }
    }

    public void SetToStart()
    {
        if (enable_position)
        {
            print(start.GetPos().x);
            target.position = new Vector3(start.GetPos().x, start.GetPos().y, start.GetPos().z);
        }
        if (enable_rotation)
        {
            target.rotation = Quaternion.Euler(start.GetRot().x, start.GetRot().y, start.GetRot().z);
        }
        if (enable_scale)
        {
            target.localScale = new Vector3(start.GetScale().x, start.GetScale().y, start.GetScale().z);
        }
    }

    public void SetToEnd()
    {
        if (enable_position)
        {
            target.position = new Vector3(end.GetPos().x, end.GetPos().y, end.GetPos().z);
        }
        if (enable_rotation)
        {
            target.rotation = Quaternion.Euler(end.GetRot().x, end.GetRot().y, end.GetRot().z);
        }
        if (enable_scale)
        {
            target.localScale = new Vector3(end.GetScale().x, end.GetScale().y, end.GetScale().z);
        }
    }

    // Use this for initialization
    void Start () {
        time = 0.0f;
        loop_times = 0;
        active = false;
        direction = -1;

        if (!target)
            target = transform;

        o_pos = new Vector3(target.position.x, target.position.y, target.position.z);
        o_rot = new Vector3(target.rotation.eulerAngles.x, target.rotation.eulerAngles.y, target.rotation.eulerAngles.z);
        o_scale = new Vector3(target.localScale.x, target.localScale.y, target.localScale.z);

        if (play_at_start)
            Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (!active)
            return;

        UpdateTime();
        UpdateTransforms();
	}

    void UpdateTransforms()
    {
        float normalized_time = time / duration;
        if (direction < 0)
            normalized_time = 1.0f - normalized_time;

        float animation_time = curve.Evaluate(normalized_time);

        Vector3 origin_pos = new Vector3(0,0,0);
        Vector3 origin_rot = Vector3.zero;
        Vector3 origin_scale = Vector3.zero;
        
        if (additive)
        {
            origin_pos = new Vector3(o_pos.x, o_pos.y, o_pos.z);
            origin_rot = new Vector3(o_rot.x, o_rot.y, o_rot.z);
            origin_scale = new Vector3(o_scale.x, o_scale.y, o_scale.z);
        }

        if (enable_position)
            target.position = origin_pos + start.GetPos() + (end.GetPos() - start.GetPos()) * animation_time;
        if (enable_rotation)
            target.rotation = Quaternion.Euler(origin_rot + start.GetRot() + (end.GetRot() - start.GetRot()) * animation_time);
        if (enable_scale)
            target.localScale = origin_scale + start.GetScale() + (end.GetScale() - start.GetScale()) * animation_time;
    }

    void UpdateTime()
    {
        time += Time.deltaTime;

        if (time > duration)
        {
            time = duration;

            if (loop == LOOP.NONE || (loop == LOOP.ONCE && loop_times > 0))
                active = false;
            else
            {
                loop_times++;
                Play(false);
            }
        }
    }
}

/*  Copyright (c) 2016 Uplause Ltd
    Original author Jalmari Ikävalko, tzaeru@gmail.com
    
    Licensed under MIT. See the associated LICENSE text file for details.

    Contributors welcome!
*/

using UnityEngine;
using System.Collections;
using UnityEngine.Events;


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

    public enum LOOP { NONE = 0, ONCE = 1, FOREVER = 2 }
    public LOOP loop = LOOP.NONE;
    public bool pingpong = true;

    public bool play_at_start = false;

    [System.Serializable]
    public class Events
    {

        [System.Serializable]
        public class StartEvent : UnityEvent<UpTween> { };
        public StartEvent start_event;

        [System.Serializable]
        public class UpdateEvent : UnityEvent<UpTween> { };
        public UpdateEvent update_event;

        [System.Serializable]
        public class EndEvent : UnityEvent<UpTween> { };
        public EndEvent end_event;
    }
    public Events events;

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

    [HideInInspector]
    public float time = 0.0f;

    bool active = false;
    int direction = -1;
    int loop_times = 0;

    public void PlayFromStart()
    {
        events.start_event.Invoke(this);
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
        if (!start.parent)
            start.parent = this;

        start.CopyStart();
    }

    public void CopyEnd()
    {
        if (!target)
            target = transform;
        if (!end.parent)
            end.parent = this;

        end.CopyStart();
    }

    public void SetToStart()
    {
        if (!target)
            target = transform;
        if (!start.parent)
            start.parent = this;

        start.SetToStart();
    }

    public void SetToEnd()
    {
        if (!target)
            target = transform;
        if (!end.parent)
            end.parent = this;

        end.SetToStart();
    }

    // Use this for initialization
    void Start () {
        // Init
        start.parent = this;
        end.parent = this;

        time = 0.0f;
        loop_times = 0;
        active = false;
        direction = -1;

        if (!target)
            target = transform;

        if (start.parent)
            start.parent = this;
        start.SetOriginalPositions();
        if (end.parent)
            end.parent = this;
        end.SetOriginalPositions();

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

        UpTweenTransformation.Update(this, start, end, animation_time);
    }

    void UpdateTime()
    {
        time += Time.deltaTime;

        events.update_event.Invoke(this);

        if (time > duration)
        {
            time = duration;

            if (loop == LOOP.NONE || (loop == LOOP.ONCE && loop_times > 0))
            {
                events.end_event.Invoke(this);
                active = false;
            }
            else
            {
                loop_times++;
                Play(false);
            }
        }
    }
}

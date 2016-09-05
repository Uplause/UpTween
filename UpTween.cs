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

    public enum Type { TRANSFORM, RECT_TRANSFORM, MATERIAL_COLOR, DYNAMIC}
    public Type type;

    public UpTweenTransformValues[] transform_values = new UpTweenTransformValues[2];
    public UpTweenRectTransformValues[] rect_transform_values = new UpTweenRectTransformValues[2];
    public UpTweenMaterialColorValues[] material_color_values = new UpTweenMaterialColorValues[2];

    public AnimationCurve curve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);

    public bool additive = true;

    public enum LOOP { NONE = 0, ONCE = 1, FOREVER = 2 }
    public LOOP loop = LOOP.NONE;
    public bool pingpong = true;

    public bool play_at_start = false;

    private UpTweenAbstractValues[] values = new UpTweenAbstractValues[2];

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

    enum Direction { LEFT, RIGHT};
    Direction direction = Direction.LEFT;

    int loop_times = 0;

    int current_value = 0;

    public void PlayFromStart()
    {
        events.start_event.Invoke(this);
        Play(true);
    }

    public void Play(bool reset_loop_times = true)
    {
        active = true;
        if (direction == Direction.LEFT)
            direction = Direction.RIGHT;
        else
            direction = Direction.LEFT;

        if (!pingpong)
            direction = Direction.RIGHT;

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

        if (!transform_values[0].parent)
            transform_values[0].parent = this;
        transform_values[0].CopyStart();

        if (!rect_transform_values[0].parent)
            rect_transform_values[0].parent = this;
        rect_transform_values[0].CopyStart();

        if (!material_color_values[0].parent)
            material_color_values[0].parent = this;
        material_color_values[0].CopyStart();
    }

    public void CopyEnd()
    {
        if (!target)
            target = transform;

        if (!transform_values[transform_values.Length - 1].parent)
            transform_values[transform_values.Length - 1].parent = this;
        transform_values[transform_values.Length - 1].CopyStart();

        if (!rect_transform_values[rect_transform_values.Length - 1].parent)
            rect_transform_values[rect_transform_values.Length - 1].parent = this;
        rect_transform_values[rect_transform_values.Length - 1].CopyStart();

        if (!material_color_values[material_color_values.Length - 1].parent)
            material_color_values[material_color_values.Length - 1].parent = this;
        material_color_values[material_color_values.Length - 1].CopyStart();
    }

    public void SetToStart()
    {
        if (!target)
            target = transform;

        if (!transform_values[0].parent)
            transform_values[0].parent = this;
        transform_values[0].SetToStart();

        if (!rect_transform_values[0].parent)
            rect_transform_values[0].parent = this;
        rect_transform_values[0].SetToStart();

        if (!material_color_values[0].parent)
            material_color_values[0].parent = this;
        material_color_values[0].SetToStart();
    }

    public void SetToEnd()
    {
        if (!target)
            target = transform;

        if (!transform_values[transform_values.Length - 1].parent)
            transform_values[transform_values.Length - 1].parent = this;
        transform_values[transform_values.Length - 1].SetToStart();

        if (!rect_transform_values[rect_transform_values.Length - 1].parent)
            rect_transform_values[rect_transform_values.Length - 1].parent = this;
        rect_transform_values[rect_transform_values.Length - 1].SetToStart();

        if (!material_color_values[rect_transform_values.Length - 1].parent)
            material_color_values[rect_transform_values.Length - 1].parent = this;
        material_color_values[rect_transform_values.Length - 1].SetToStart();
    }

    // Use this for initialization
    void Start () {
        if (!target)
            target = transform;

        SetupValueArray();

        if (!target)
            target = transform;

        if (play_at_start)
            Play();
    }

    void SetupValueArray()
    {
        if (type == Type.TRANSFORM)
            values = transform_values.Clone() as UpTweenAbstractValues[];
        else if (type == Type.RECT_TRANSFORM)
            values = rect_transform_values.Clone() as UpTweenAbstractValues[];
        else if (type == Type.MATERIAL_COLOR)
            values = material_color_values.Clone() as UpTweenAbstractValues[];

        foreach (var value in values)
        {
            value.parent = this;
            value.SetOriginalPositions();
        }
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
        float normalized_time = time / values[current_value].duration;
        if (direction == Direction.LEFT)
            normalized_time = 1.0f - normalized_time;

        if (normalized_time > 1.0f)
            normalized_time = 1.0f;

        float animation_time = curve.Evaluate(normalized_time);

        if (direction == Direction.RIGHT)
            values[0].Update(this, values[current_value], values[current_value+1], animation_time);
        else
            values[0].Update(this, values[current_value], values[current_value+1], animation_time);
    }

    void UpdateTime()
    {
        time += Time.deltaTime;

        events.update_event.Invoke(this);

        if (time > values[current_value].duration)
        {
            time = values[current_value].duration;

            if ((direction == Direction.RIGHT && current_value >= values.Length - 2)
                || (direction == Direction.LEFT && current_value <= 1))
            {
                print("Does it do this");
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
            else
            {
                time = 0.0f;
                current_value++;
            }
        }
    }
}

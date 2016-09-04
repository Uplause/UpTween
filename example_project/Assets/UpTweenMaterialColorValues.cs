using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class UpTweenMaterialColorValues : UpTweenAbstractValues
{
    public Transform target;

    public Color color;

    // Original transformation
    [HideInInspector]
    public Color o_color;

    public override void SetToStart()
    {
        if (parent.target.GetComponent<Renderer>())
            parent.target.GetComponent<Renderer>().material.color = color;
        else if (parent.target.GetComponent<Image>())
            parent.target.GetComponent<Image>().color = color;
    }

    public override void CopyStart()
    {
        if (parent.target.GetComponent<Renderer>())
            color = parent.target.GetComponent<Renderer>().material.color;
        else if (parent.target.GetComponent<Image>())
            color = parent.target.GetComponent<Image>().color;
    }

    public override void SetOriginalPositions()
    {
        if (parent.target.GetComponent<Renderer>())
            o_color = parent.target.GetComponent<Renderer>().material.color;
        else if (parent.target.GetComponent<Image>())
            o_color = parent.target.GetComponent<Image>().color;
    }

    public override void Update(UpTween target, UpTweenAbstractValues _A, UpTweenAbstractValues _B, float animation_time)
    {
        UpTweenMaterialColorValues A = (UpTweenMaterialColorValues)_A;
        UpTweenMaterialColorValues B = (UpTweenMaterialColorValues)_B;
        
        Color origin_color = Vector4.zero;

        if (target.additive)
        {
            origin_color = new Vector4(A.o_color.r, A.o_color.g, A.o_color.b, A.o_color.a);
        }

        if (parent.target.GetComponent<Renderer>())
            parent.target.GetComponent<Renderer>().material.color = origin_color + A.color + (B.color - A.color) * animation_time;
        else if (parent.target.GetComponent<Image>())
            parent.target.GetComponent<Image>().color = origin_color + A.color + (B.color - A.color) * animation_time;
    }
}
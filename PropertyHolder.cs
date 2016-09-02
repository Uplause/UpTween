using UnityEngine;
using System.Collections;

public class PropertyHolder : MonoBehaviour
{
    public enum Status { A, B, C };

    public Status state;

    public int valForAB;

    public int valForA;
    public int valForC;

    public bool controllable;

    void Start()
    {

    }

    void Update()
    {

    }
}
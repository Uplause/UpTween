using UnityEngine;
using System.Collections;

public class EventTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Ended(UpTween tween)
    {
        print("Tweening ended"); ;
    }

    public void Update(UpTween tween)
    {
        print("Tweening updated: " + tween.time);
    }

    public void Started(UpTween tween)
    {
        print("Tweening started");
    }
}

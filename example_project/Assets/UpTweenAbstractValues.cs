﻿using UnityEngine;
using System.Collections;

public abstract class UpTweenAbstractValues {
    [HideInInspector]
    public UpTween parent;

    public abstract void SetToStart();

    public abstract void CopyStart();

    public abstract void SetOriginalPositions();

    public abstract void Update(UpTween target, UpTweenAbstractValues A, UpTweenAbstractValues B, float animation_time);
}

# UpTween
A simple Unity tweening library for both the UI and ordinary objects.

![alt tag](http://i.imgur.com/kt8RMyN.png)

# Features

* **Additive tweening** - Instead of setting where to tween an object, you can set how much to tween an object. You can also set absolute, non-relative targets if you wish to.
* **Both UI and non-UI objects** - Works with any Unity gameObject that has a transform component.
* **Uses the curve component** - Draw any kind of a tween curve you want to.
* **Inspector integration** - All values can be set and tweens can be started, stopped and continued via the inspector.
* **Looping, pingpong, etc** - All the usual goodies!
* **Clean, straight-forward codebase** - Or so I hope.

# Use

To install the library, simply place "UpTween.cs" and "InspectorButton.cs" to your project's asset directory. You can then use it by  adding "Up Tween" component to your game objects.

You can set "start" and "end" values to either point to another Transformation, or you can leave "end" and "start" targets empty and instead fill in values manually.

To test your tween, start the project and click "Start tween" via the inspector.

And check out the example scene!

# Missing features

Currently, at least the following features are missing:

* Running tweens outside play mode
* Using mathematical functions instead of manually drawn curves
* Setting Rect Transform values.

# License

MIT. For details, see the LICENSE file.

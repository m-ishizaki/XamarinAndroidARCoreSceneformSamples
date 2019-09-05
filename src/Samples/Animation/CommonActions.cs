using Android.Views;
using Com.Google.AR.Sceneform;
using Google.AR.Core;
using System;
using static Android.Views.View;
using static Com.Google.AR.Sceneform.Scene;
using static Com.Google.AR.Sceneform.UX.BaseArFragment;

namespace Animation
{
    class Consumer : Java.Lang.Object, Java.Util.Functions.IConsumer
    {
        Action<Java.Lang.Object> _action;

        public Consumer(Action<Java.Lang.Object> action) => _action = action;

        public void Accept(Java.Lang.Object t) => _action?.Invoke(t);
    }

    class OnTapArPlaneListener : Java.Lang.Object, IOnTapArPlaneListener
    {
        Action<HitResult, Plane, MotionEvent> _action;

        public OnTapArPlaneListener(Action<HitResult, Plane, MotionEvent> action) => _action = action;

        public void OnTapPlane(HitResult hitResult, Plane plane, MotionEvent motionEvent) => _action?.Invoke(hitResult, plane, motionEvent);
    }

    class Function : Java.Lang.Object, Java.Util.Functions.IFunction
    {
        Func<Java.Lang.Object, Java.Lang.Object> _action;

        public Function(Func<Java.Lang.Object, Java.Lang.Object> action) => _action = action;

        public Java.Lang.Object Apply(Java.Lang.Object t) => _action?.Invoke(t);
    }

    class OnClickListner : Java.Lang.Object, IOnClickListener
    {
        Action<View> _action;

        public OnClickListner(Action<View> action) => _action = action;

        public void OnClick(View v) => _action?.Invoke(v);
    }

    class OnUpdateListener : Java.Lang.Object, IOnUpdateListener
    {
        Action<FrameTime> _action;

        public OnUpdateListener(Action<FrameTime> action) => _action = action;

        public void OnUpdate(FrameTime frameTime) => _action?.Invoke(frameTime);
    }
}
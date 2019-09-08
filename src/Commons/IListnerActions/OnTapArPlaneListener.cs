using Android.Views;
using Google.AR.Core;
using System;
using static Google.AR.Sceneform.UX.BaseArFragment;

namespace IListnerActions
{
    public class OnTapArPlaneListener : Java.Lang.Object, IOnTapArPlaneListener
    {
        Action<HitResult, Plane, MotionEvent> _action;

        public OnTapArPlaneListener(Action<HitResult, Plane, MotionEvent> action) => _action = action;

        public void OnTapPlane(HitResult hitResult, Plane plane, MotionEvent motionEvent) => _action?.Invoke(hitResult, plane, motionEvent);
    }
}
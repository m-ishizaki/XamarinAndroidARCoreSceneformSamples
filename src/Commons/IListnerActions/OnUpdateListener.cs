using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.AR.Sceneform;
using static Google.AR.Sceneform.Scene;

namespace IListnerActions
{
    public class OnUpdateListener : Java.Lang.Object, IOnUpdateListener
    {
        Action<FrameTime> _action;

        public OnUpdateListener(Action<FrameTime> action) => _action = action;

        public void OnUpdate(FrameTime frameTime) => _action?.Invoke(frameTime);
    }
}
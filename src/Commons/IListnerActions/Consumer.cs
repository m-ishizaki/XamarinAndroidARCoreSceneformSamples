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

namespace IListnerActions
{
    public class Consumer : Java.Lang.Object, Java.Util.Functions.IConsumer
    {
        Action<Java.Lang.Object> _action;

        public Consumer(Action<Java.Lang.Object> action) => _action = action;

        public void Accept(Java.Lang.Object t) => _action?.Invoke(t);
    }
}
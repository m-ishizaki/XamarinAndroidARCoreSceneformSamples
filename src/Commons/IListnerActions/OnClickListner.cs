using Android.Views;
using System;
using static Android.Views.View;

namespace IListnerActions
{
    public class OnClickListner : Java.Lang.Object, IOnClickListener
    {
        Action<View> _action;

        public OnClickListner(Action<View> action) => _action = action;

        public void OnClick(View v) => _action?.Invoke(v);
    }
}
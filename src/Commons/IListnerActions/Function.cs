using System;

namespace IListnerActions
{
    public class Function : Java.Lang.Object, Java.Util.Functions.IFunction
    {
        Func<Java.Lang.Object, Java.Lang.Object> _action;

        public Function(Func<Java.Lang.Object, Java.Lang.Object> action) => _action = action;

        public Java.Lang.Object Apply(Java.Lang.Object t) => _action?.Invoke(t);
    }
}
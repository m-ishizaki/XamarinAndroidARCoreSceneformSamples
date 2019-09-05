using Android.Runtime;
using Java.Interop;

namespace Com.Google.AR.Sceneform.Animation
{
    public partial class ModelAnimator
    {

        public override unsafe long StartDelay
        {
            // Metadata.xml XPath method reference: path="/api/package[@name='com.google.ar.sceneform.animation']/class[@name='ModelAnimator']/method[@name='getStartDelay' and count(parameter)=0]"
            [Register("getStartDelay", "()J", "GetGetStartDelayHandler")]
            get
            {
                const string __id = "getStartDelay.()J";
                try
                {
                    var __rm = _members.InstanceMethods.InvokeVirtualInt64Method(__id, this, null);
                    return __rm;
                }
                finally
                {
                }
            }
            set
            {
                SetStartDelay(value);
            }
        }

        // Metadata.xml XPath method reference: path="/api/package[@name='com.google.ar.sceneform.animation']/class[@name='ModelAnimator']/method[@name='setStartDelay' and count(parameter)=1 and parameter[1][@type='long']]"
        [Register("setStartDelay", "(J)V", "GetSetStartDelay_JHandler")]
        private unsafe void SetStartDelay(long p0)
        {
            const string __id = "setStartDelay.(J)V";
            try
            {
                JniArgumentValue* __args = stackalloc JniArgumentValue[1];
                __args[0] = new JniArgumentValue(p0);
                _members.InstanceMethods.InvokeVirtualVoidMethod(__id, this, __args);
            }
            finally
            {
            }
        }

    }
}
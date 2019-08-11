/*
 * Copyright 2018 Google LLC. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/*
* 2019 m-ishizaki
* Xamarin.Android のサンプル用に C# コードに変更
*/

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Google.AR.Core;
using Com.Google.AR.Sceneform;
using Com.Google.AR.Sceneform.Rendering;
using Com.Google.AR.Sceneform.UX;
using System;
using static Com.Google.AR.Sceneform.UX.BaseArFragment;

namespace HelloSceneform
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private static string TAG { get; } = typeof(MainActivity).Name;
        private const double MIN_OPENGL_VERSION = 3.0;

        private ArFragment arFragment;
        public ModelRenderable andyRenderable;

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

        // CompletableFuture requires api level 24
        // FutureReturnValueIgnored is not valid
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!CheckIsSupportedDeviceOrFinish(this))
            {
                return;
            }

            SetContentView(Resource.Layout.activity_main);
            arFragment = (ArFragment)SupportFragmentManager.FindFragmentById(Resource.Id.ux_fragment);

            // When you build a Renderable, Sceneform loads its resources in the background while returning
            // a CompletableFuture. Call thenAccept(), handle(), or check isDone() before calling get().
            ModelRenderable.CreateBuilder()
            .SetSource(this, Resource.Raw.andy)
                .Build()
                .ThenAccept(new Consumer(t => andyRenderable = (ModelRenderable)t))
                .Exceptionally(new Function(_ =>
                {
                    Toast toast = Toast.MakeText(this, "Unable to load andy renderable", ToastLength.Long);
                    toast.SetGravity(GravityFlags.Center, 0, 0);
                    toast.Show();
                    return null;
                }
                ));

            arFragment.SetOnTapArPlaneListener(new OnTapArPlaneListener((hitResult, plane, motionEvent) =>
            {
                if (andyRenderable == null) { return; }

                // Create the Anchor.
                Com.Google.AR.Core.Anchor anchor = hitResult.CreateAnchor();
                AnchorNode anchorNode = new AnchorNode(anchor);
                anchorNode.SetParent(arFragment.ArSceneView.Scene);

                // Create the transformable andy and add it to the anchor.
                TransformableNode andy = new TransformableNode(arFragment.TransformationSystem);
                andy.SetParent(anchorNode);
                andy.Renderable = andyRenderable;
                andy.Select();
            }
            ));
        }

        /**
         * Returns false and displays an error message if Sceneform can not run, true if Sceneform can run
         * on this device.
         *
         * <p>Sceneform requires Android N on the device as well as OpenGL 3.0 capabilities.
         *
         * <p>Finishes the activity if Sceneform can not run
         */
        public static bool CheckIsSupportedDeviceOrFinish(Activity activity)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.N)
            {
                Log.Error(TAG, "Sceneform requires Android N or later");
                Toast.MakeText(activity, "Sceneform requires Android N or later", ToastLength.Long).Show();
                activity.Finish();
                return false;
            }
            string openGlVersionString =
                ((ActivityManager)activity.GetSystemService(Context.ActivityService))
                    .DeviceConfigurationInfo
                    .GlEsVersion;
            if (double.Parse(openGlVersionString) < MIN_OPENGL_VERSION)
            {
                Log.Error(TAG, "Sceneform requires OpenGL ES 3.0 later");
                Toast.MakeText(activity, "Sceneform requires OpenGL ES 3.0 or later", ToastLength.Long)
                    .Show();
                activity.Finish();
                return false;
            }
            return true;
        }
    }
}
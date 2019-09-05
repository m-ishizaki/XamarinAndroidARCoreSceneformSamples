/*
 * Copyright 2018 Google LLC
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
using Android.Content.Res;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Google.AR.Sceneform;
using Com.Google.AR.Sceneform.Animation;
using Com.Google.AR.Sceneform.Math;
using Com.Google.AR.Sceneform.Rendering;
using Com.Google.AR.Sceneform.UX;
using Google.AR.Core;
using Java.Lang;

namespace Animation
{
    /** Demonstrates playing animated FBX models. */
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private const string TAG = "AnimationSample";
        private const int ANDY_RENDERABLE = 1;
        private const int HAT_RENDERABLE = 2;
        private const string HAT_BONE_NAME = "hat_point";
        private ArFragment arFragment;
        // Model loader class to avoid leaking the activity context.
        private ModelLoader modelLoader;
        private ModelRenderable andyRenderable;
        private AnchorNode anchorNode;
        private SkeletonNode andy;
        // Controls animation playback.
        private ModelAnimator animator;
        // Index of the current animation playing.
        private int nextAnimation;
        // The UI to play next animation.
        private FloatingActionButton animationButton;
        // The UI to toggle wearing the hat.
        private FloatingActionButton hatButton;
        private Node hatNode;
        private ModelRenderable hatRenderable;

        //@SuppressWarnings({ "AndroidApiChecker", "FutureReturnValueIgnored"})
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            arFragment = (ArFragment)SupportFragmentManager.FindFragmentById(Resource.Id.sceneform_fragment);

            modelLoader = new ModelLoader(this);

            modelLoader.LoadModel(ANDY_RENDERABLE, Resource.Raw.andy_dance);
            modelLoader.LoadModel(HAT_RENDERABLE, Resource.Raw.baseball_cap);

            // When a plane is tapped, the model is placed on an Anchor node anchored to the plane.
            arFragment.SetOnTapArPlaneListener(new OnTapArPlaneListener(this.OnPlaneTap));

            // Add a frame update listener to the scene to control the state of the buttons.
            arFragment.ArSceneView.Scene.AddOnUpdateListener(new OnUpdateListener(this.OnFrameUpdate));

            // Once the model is placed on a plane, this button plays the animations.
            animationButton = FindViewById<FloatingActionButton>(Resource.Id.animate);
            animationButton.Enabled = false;
            animationButton.SetOnClickListener(new OnClickListner(this.OnPlayAnimation));

            // Place or remove a hat on Andy's head showing how to use Skeleton Nodes.
            hatButton = FindViewById<FloatingActionButton>(Resource.Id.hat);
            hatButton.Enabled = false;
            hatButton.SetOnClickListener(new OnClickListner(this.OnToggleHat));
        }

        private void OnPlayAnimation(View unusedView)
        {
            if (animator == null || !animator.IsRunning)
            {
                AnimationData data = andyRenderable.GetAnimationData(nextAnimation);
                nextAnimation = (nextAnimation + 1) % andyRenderable.AnimationDataCount;
                animator = new ModelAnimator(data, andyRenderable);
                animator.Start();
                Toast toast = Toast.MakeText(this, data.Name, ToastLength.Short);
                Log.Debug(
                    TAG,
                    string.Format(
                        "Starting animation %s - %d ms long", data.Name, data.DurationMs));
                toast.SetGravity(GravityFlags.Center, 0, 0);
                toast.Show();
            }
        }

        /*
         * Used as the listener for setOnTapArPlaneListener.
         */
        private void OnPlaneTap(HitResult hitResult, Google.AR.Core.Plane unusedPlane, MotionEvent unusedMotionEvent)
        {
            if (andyRenderable == null || hatRenderable == null)
            {
                return;
            }
            // Create the Anchor.
            Anchor anchor = hitResult.CreateAnchor();

            if (anchorNode == null)
            {
                anchorNode = new AnchorNode(anchor);
                anchorNode.SetParent(arFragment.ArSceneView.Scene);

                andy = new SkeletonNode();

                andy.SetParent(anchorNode);
                andy.Renderable = andyRenderable;
                hatNode = new Node();

                // Attach a node to the bone.  This node takes the internal scale of the bone, so any
                // renderables should be added to child nodes with the world pose reset.
                // This also allows for tweaking the position relative to the bone.
                Node boneNode = new Node();
                boneNode.SetParent(andy);
                andy.SetBoneAttachment(HAT_BONE_NAME, boneNode);
                hatNode.Renderable = hatRenderable;
                hatNode.SetParent(boneNode);
                hatNode.WorldScale = Vector3.One();
                hatNode.WorldRotation = Quaternion.Identity();
                Vector3 pos = hatNode.WorldPosition;

                // Lower the hat down over the antennae.
                pos.Y -= .1f;

                hatNode.WorldPosition = pos;
            }
        }
        /**
         * Called on every frame, control the state of the buttons.
         *
         * @param unusedframeTime
         */
        private void OnFrameUpdate(FrameTime unusedframeTime)
        {
            // If the model has not been placed yet, disable the buttons.
            if (anchorNode == null)
            {
                if (animationButton.Enabled)
                {
                    animationButton.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Gray);
                    animationButton.Enabled = false;
                    hatButton.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Gray);
                    hatButton.Enabled = false;
                }
            }
            else
            {
                if (!animationButton.Enabled)
                {
                    animationButton.BackgroundTintList =
                        ColorStateList.ValueOf(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.colorAccent)));
                    animationButton.Enabled = true;
                    hatButton.Enabled = true;
                    hatButton.BackgroundTintList =
                        ColorStateList.ValueOf(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.colorPrimary)));
                }
            }
        }

        private void OnToggleHat(View unused)
        {
            if (hatNode != null)
            {
                hatNode.Enabled = !hatNode.Enabled;

                // Set the state of the hat button based on the hat node.
                if (hatNode.Enabled)
                {
                    hatButton.BackgroundTintList =
                        ColorStateList.ValueOf(
                           new Android.Graphics.Color(
                            ContextCompat.GetColor(this, Resource.Color.colorPrimary)));
                }
                else
                {
                    hatButton.BackgroundTintList =
                        ColorStateList.ValueOf(new Android.Graphics.Color(ContextCompat.GetColor(this, Resource.Color.colorAccent)));
                }
            }
        }

        internal void SetRenderable(int id, ModelRenderable renderable)
        {
            if (id == ANDY_RENDERABLE)
            {
                this.andyRenderable = renderable;
            }
            else
            {
                this.hatRenderable = renderable;
            }
        }

        internal void OnException(int id, Throwable throwable)
        {
            Toast toast = Toast.MakeText(this, "Unable to load renderable: " + id, ToastLength.Long);
            toast.SetGravity(GravityFlags.Center, 0, 0);
            toast.Show();
            Log.Error(TAG, "Unable to load andy renderable", throwable);
        }
    }
}
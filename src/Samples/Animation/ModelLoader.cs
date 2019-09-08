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

using Android.Runtime;
using Android.Util;
using Google.AR.Sceneform.Rendering;
using IListnerActions;
using Java.Lang;
using Java.Util.Concurrent;

namespace Animation
{

    /**
     * Model loader class to avoid memory leaks from the activity. Activity and Fragment controller
     * classes have a lifecycle that is controlled by the UI thread. When a reference to one of these
     * objects is accessed by a background thread it is "leaked". Using that reference to a
     * lifecycle-bound object after Android thinks it has "destroyed" it can produce bugs. It also
     * prevents the Activity or Fragment from being garbage collected, which can leak the memory
     * permanently if the reference is held in the singleton scope.
     *
     * <p>To avoid this, use a non-nested class which is not an activity nor fragment. Hold a weak
     * reference to the activity or fragment and use that when making calls affecting the UI.
     */
    class ModelLoader
    {
        private const string TAG = "ModelLoader";
        private SparseArray<CompletableFuture> futureSet { get; } = new SparseArray<CompletableFuture>();
        private Java.Lang.Ref.WeakReference owner;

        public ModelLoader(MainActivity owner)
        {
            this.owner = new Java.Lang.Ref.WeakReference(owner);
        }

        /**
         * Starts loading the model specified. The result of the loading is returned asynchrounously via
         * {@link MainActivity#setRenderable(int, ModelRenderable)} or {@link
         * MainActivity#onException(int, Throwable)}.
         *
         * <p>Multiple models can be loaded at a time by specifying separate ids to differentiate the
         * result on callback.
         *
         * @param id the id for this call to loadModel.
         * @param resourceId the resource id of the .sfb to load.
         * @return true if loading was initiated.
         */
        internal bool LoadModel(int id, int resourceId)
        {
            MainActivity activity = (MainActivity)owner.Get();
            if (activity == null)
            {
                Log.Debug(TAG, "Activity is null.  Cannot load model.");
                return false;
            }
            CompletableFuture future =
                (CompletableFuture)ModelRenderable.InvokeBuilder()
                    .SetSource((MainActivity)owner.Get(), resourceId)
                    .Build()
                    .ThenApply(new Function(renderable => this.SetRenderable(id, (ModelRenderable)renderable)))
                    .Exceptionally(new Function(throwable => this.OnException(id, (Throwable)(IJavaObject)throwable)));
            if (future != null)
            {
                futureSet.Put(id, future);
            }
            return future != null;
        }

        ModelRenderable OnException(int id, Throwable throwable)
        {
            MainActivity activity = (MainActivity)owner.Get();
            if (activity != null)
            {
                activity.OnException(id, throwable);
            }
            futureSet.Remove(id);
            return null;
        }

        ModelRenderable SetRenderable(int id, ModelRenderable modelRenderable)
        {
            MainActivity activity = (MainActivity)owner.Get();
            if (activity != null)
            {
                activity.SetRenderable(id, modelRenderable);
            }
            futureSet.Remove(id);
            return modelRenderable;
        }
    }


}
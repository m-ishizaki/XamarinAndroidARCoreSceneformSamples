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

using Android;
using Google.AR.Sceneform.UX;


namespace Animation
{
    /**
     * Writing Ar Fragment extends the ArFragment class to include the WRITER_EXTERNAL_STORAGE
     * permission. This adds this permission to the list of permissions presented to the user for
     * granting.
     */
    public class WritingArFragment : ArFragment
    {
        public override string[] GetAdditionalPermissions()
        {
            string[] additionalPermissions = base.GetAdditionalPermissions();
            int permissionLength = additionalPermissions != null ? additionalPermissions.Length : 0;
            string[] permissions = new string[permissionLength + 1];
            permissions[0] = Manifest.Permission.WriteExternalStorage;
            if (permissionLength > 0)
            {
                System.Array.Copy(additionalPermissions, 0, permissions, 1, additionalPermissions.Length);
            }
            return permissions;
        }
    }
}

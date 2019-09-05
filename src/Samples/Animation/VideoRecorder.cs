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

using Android.Media;
using Android.OS;
using Android.Util;
using Android.Views;
using Com.Google.AR.Sceneform;
using Java.IO;

namespace Animation
{
    /**
     * Video Recorder class handles recording the contents of a SceneView. It uses MediaRecorder to
     * encode the video. The quality settings can be set explicitly or simply use the CamcorderProfile
     * class to select a predefined set of parameters.
     */
    public class VideoRecorder
    {
        private const string TAG = "VideoRecorder";
        private const int DEFAULT_BITRATE = 10000000;
        private const int DEFAULT_FRAMERATE = 30;

        // recordingVideoFlag is true when the media recorder is capturing video.
        private bool recordingVideoFlag;

        private MediaRecorder mediaRecorder;

        private Size videoSize;

        private SceneView sceneView;
        private VideoEncoder videoCodec;
        private File videoDirectory;
        private string videoBaseName;
        private File videoPath;
        private int bitRate = DEFAULT_BITRATE;
        private int frameRate = DEFAULT_FRAMERATE;
        private Surface encoderSurface;

        public VideoRecorder()
        {
            recordingVideoFlag = false;
        }

        public File GetVideoPath()
        {
            return videoPath;
        }

        public void SetBitRate(int bitRate)
        {
            this.bitRate = bitRate;
        }

        public void SetFrameRate(int frameRate)
        {
            this.frameRate = frameRate;
        }

        public void SetSceneView(SceneView sceneView)
        {
            this.sceneView = sceneView;
        }

        /**
         * Toggles the state of video recording.
         *
         * @return true if recording is now active.
         */
        public bool OnToggleRecord()
        {
            if (recordingVideoFlag)
            {
                StopRecordingVideo();
            }
            else
            {
                StartRecordingVideo();
            }
            return recordingVideoFlag;
        }

        private void StartRecordingVideo()
        {
            if (mediaRecorder == null)
            {
                mediaRecorder = new MediaRecorder();
            }

            try
            {
                BuildFilename();
                SetUpMediaRecorder();
            }
            catch (IOException e)
            {
                Log.Error(TAG, "Exception setting up recorder", e);
                return;
            }

            // Set up Surface for the MediaRecorder
            encoderSurface = mediaRecorder.Surface;

            sceneView.StartMirroringToSurface(
                encoderSurface, 0, 0, videoSize.Width, videoSize.Height);

            recordingVideoFlag = true;
        }

        private void BuildFilename()
        {
            if (videoDirectory == null)
            {
                videoDirectory =
                    new File(
                        Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures)
                            + "/Sceneform");
            }
            if (string.IsNullOrEmpty(videoBaseName))
            {
                videoBaseName = "Sample";
            }
            videoPath =
                new File(
                    videoDirectory, videoBaseName + Java.Lang.Long.ToHexString(Java.Lang.JavaSystem.CurrentTimeMillis()) + ".mp4");
            File dir = videoPath.ParentFile;
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }
        }

        private void StopRecordingVideo()
        {
            // UI
            recordingVideoFlag = false;

            if (encoderSurface != null)
            {
                sceneView.StopMirroringToSurface(encoderSurface);
                encoderSurface = null;
            }
            // Stop recording
            mediaRecorder.Stop();
            mediaRecorder.Reset();
        }

        private void SetUpMediaRecorder()
        {
            mediaRecorder.SetVideoSource(VideoSource.Surface);
            mediaRecorder.SetOutputFormat(OutputFormat.Mpeg4);

            mediaRecorder.SetOutputFile(videoPath.AbsolutePath);
            mediaRecorder.SetVideoEncodingBitRate(bitRate);
            mediaRecorder.SetVideoFrameRate(frameRate);
            mediaRecorder.SetVideoSize(videoSize.Width, videoSize.Height);
            mediaRecorder.SetVideoEncoder(videoCodec);

            mediaRecorder.Prepare();

            try
            {
                mediaRecorder.Start();
            }
            catch (Java.Lang.IllegalStateException e)
            {
                Log.Error(TAG, "Exception starting capture: " + e.Message, e);
            }
        }

        public void SetVideoSize(int width, int height)
        {
            videoSize = new Size(width, height);
        }

        public void SetVideoQuality(CamcorderQuality quality, Android.Content.Res.Orientation orientation)
        {
            CamcorderProfile profile = CamcorderProfile.Get(quality);
            if (profile == null)
            {
                profile = CamcorderProfile.Get(CamcorderQuality.High);
            }
            if (orientation == Android.Content.Res.Orientation.Landscape)
            {
                SetVideoSize(profile.VideoFrameWidth, profile.VideoFrameHeight);
            }
            else
            {
                SetVideoSize(profile.VideoFrameHeight, profile.VideoFrameWidth);
            }
            SetVideoCodec(profile.VideoCodec);
            SetBitRate(profile.VideoBitRate);
            SetFrameRate(profile.VideoFrameRate);
        }

        public void SetVideoCodec(VideoEncoder videoCodec)
        {
            this.videoCodec = videoCodec;
        }
    }
}
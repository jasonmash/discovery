using Assets.IfM.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Assets.IfM.Services
{
    public class ImageCaptureService
    {

        PhotoCapture photoCaptureObject = null;
        GameObject imagePreview;
        Texture2D targetTexture = null;

        private static readonly HttpClient client = new HttpClient();


        public Action<string> completionCallback;

        Defect defect;
        Transform transform;

        public void CapturePhoto(Transform t)
        {
            transform = t;
            Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            targetTexture = new Texture2D(cameraResolution.width / 2, cameraResolution.height / 2);
            // Create a PhotoCapture object
            PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
                photoCaptureObject = captureObject;
                CameraParameters cameraParameters = new CameraParameters();
                cameraParameters.hologramOpacity = 0.0f;
                cameraParameters.cameraResolutionWidth = cameraResolution.width / 2;
                cameraParameters.cameraResolutionHeight = cameraResolution.height / 2;
                cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

                // Activate the camera
                photoCaptureObject.StartPhotoModeAsync(cameraParameters, delegate (PhotoCapture.PhotoCaptureResult result) {
                    // Take a picture
                    photoCaptureObject.TakePhotoAsync(OnCapturedPhotoToMemory);
                });
            });
        }

        void OnCapturedPhotoToMemory(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            // Copy the raw image data into the target texture
            photoCaptureFrame.UploadImageDataToTexture(targetTexture);

            // Create a GameObject to which the texture can be applied
            if (imagePreview == null)
            {
                imagePreview = GameObject.CreatePrimitive(PrimitiveType.Quad);
            }
            Renderer quadRenderer = imagePreview.GetComponent<Renderer>() as Renderer;
            quadRenderer.material = new Material(Shader.Find("Mixed Reality Toolkit/Standard"));

            imagePreview.transform.parent = this.transform;
            imagePreview.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            imagePreview.transform.localPosition = new Vector3(0.0f, 0.0f, 1.0f);

            quadRenderer.material.SetTexture("_MainTex", targetTexture);

            // Deactivate the camera
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);

            byte[] bytes = targetTexture.EncodeToPNG();
            base64Image = Convert.ToBase64String(bytes);

            completionCallback(base64Image);

        }


        public string base64Image;

        void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
        {
            // Shutdown the photo capture resource
            photoCaptureObject.Dispose();
            photoCaptureObject = null;
        }
    }
}

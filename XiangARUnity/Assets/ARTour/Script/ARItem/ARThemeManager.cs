using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Expect.ARTour
{
    public class ARThemeManager : MonoBehaviour
    {
        [SerializeField]
        private ARThemeItem themeItem;

        [SerializeField]
        private ARTrackedImageManager imageTracker;

        private ARTrackedImage _trackImage; 

        void Start()
        {
            imageTracker.trackedImagesChanged += OnTrackedImagesChanged;
        }

        private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs e)
        {
            foreach (var trackedImage in e.added)
            {
                //Debug.Log($"Tracked image detected: {trackedImage.referenceImage.name} with size: {trackedImage.size}");
            }

            UpdateTrackedImages(e.added);
            UpdateTrackedImages(e.updated);
        }

        private void UpdateTrackedImages(List<ARTrackedImage> trackedImages)
        {
            // If the same image (ReferenceImageName)
            if (trackedImages == null || trackedImages.Count <= 0) return;

            var trackedImage = trackedImages[0];
            if (trackedImage == null)
            {
                return;
            }


            if (trackedImage.trackingState == TrackingState.Tracking)
            {
             //   Debug.Log($"Tracked image detected: {trackedImage.referenceImage.name} with position: {trackedImage.transform.position}, State {trackedImage.trackingState}");


                var trackedImageTransform = trackedImage.transform;

                themeItem.UpdateThemeWorldPosData(trackedImage.referenceImage.name, trackedImageTransform.position, trackedImageTransform.rotation);
                themeItem.ShowARTheme(trackedImage.referenceImage.name);

                if (GeneralFlag.SThemeLookUpTable.TryGetValue(trackedImage.referenceImage.name, out GeneralFlag.ARTourTheme theme)) {
                    if (_trackImage == null || _trackImage.referenceImage.name != trackedImage.referenceImage.name)
                        MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.ThemeChange, theme);
                }

                _trackImage = trackedImage;
            }
        }

        private void OnDestroy()
        {
            imageTracker.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }
}
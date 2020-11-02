using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Expect.ARTour
{
    public class ARThemeItem : MonoBehaviour
    {
        [SerializeField]
        private ARThemeAnchor[] ThemeItems;

        private Vector3 recordPosition;
        private float limitLengthDiff = 0.4f;

        void Start()
        {
            SoundTextTourCtrl.OnDataSync += (OnARDataUpdate);

        }

        private void OnARDataUpdate(ARDataSync aRDataSync)
        {
            if (aRDataSync == null)
            {
                Debug.LogError("_ARTrackedImage is null");
                return;
            }

            //Debug.Log("Reference Image name " + _ARTrackedImage.referenceImage.name);

            foreach (ARThemeAnchor aRThemeAnchor in ThemeItems) {
                ARDataSync.ARData data = aRDataSync.FindArData(aRThemeAnchor.name);

                aRThemeAnchor.SetLocalData(data.position, data.rotation, data.scale);
            }
        }

        public void UpdateThemeWorldPosData(string imageName, Vector3 worldPos, Quaternion rotation) {

            float diff = Vector3.Distance(worldPos, recordPosition);
            recordPosition = worldPos;

            if (diff > limitLengthDiff) {
                return;
            }

            var anchor = ThemeItems.FirstOrDefault<ARThemeAnchor>(x => x.name == imageName);
            if (anchor == null) return;

            anchor.transform.rotation = Quaternion.Lerp(anchor.transform.rotation, rotation, 0.1f);
            anchor.transform.position = Vector3.Lerp(anchor.transform.position, worldPos, 0.1f);
            //anchor.transform.SetPositionAndRotation(worldPos, rotation);
        }

        public void ShowARTheme(string themeName) {
            foreach (ARThemeAnchor themeItem in ThemeItems) {
                    themeItem.gameObject.SetActive(themeItem.name == themeName);
            }
        }

        private void OnDestroy()
        {
            SoundTextTourCtrl.OnDataSync -= (OnARDataUpdate);
        }
    }
}
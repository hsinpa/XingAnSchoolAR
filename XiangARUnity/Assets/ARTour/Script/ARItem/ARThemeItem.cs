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

        private string currentTheme;
        private int lockCount, lockThreshold = 40;
        private bool isLock;
        private float lockResetTime = 10;
        private float lockResetPending;

        private enum CountType {PendCount =-2, StartCount = -1, Normal = 0 }

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

            if (LockPositionRoation(imageName))
                return;

            var anchor = ThemeItems.FirstOrDefault<ARThemeAnchor>(x => x.name == imageName);
            if (anchor == null) return;

            //anchor.transform.rotation = Quaternion.Lerp(anchor.transform.rotation, rotation, 0.1f);
            //anchor.transform.position = Vector3.Lerp(anchor.transform.position, worldPos, 0.2f);
            anchor.transform.SetPositionAndRotation(worldPos, rotation);
        }

        private bool LockPositionRoation(string imageName) {
            lockResetPending = Time.time + lockResetTime;

            //Unlock
            if (currentTheme != imageName) {
                currentTheme = imageName;
                lockCount = -1;
            }

            if (lockCount == (int)CountType.PendCount)
            {
                return true;
            }

            lockCount++;

            if ((lockCount > lockThreshold)) {
                isLock = !isLock;

                //When lock, take longer time to unlock
                lockCount = (isLock) ? (int)CountType.PendCount : (int)CountType.StartCount;
            }

            return isLock;
        }

        public void Update()
        {
            if (lockResetPending < Time.time) {
                lockCount = (int)CountType.StartCount;
            }            
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hsinpa.Input {

    public class InputWrapper : MonoBehaviour
    {
        [SerializeField]
        private Camera standaloneAsset;

        [SerializeField]
        private GameObject waveAsset;

#if UNITY_EDITOR || UNITY_ANDROID
        [SerializeField]
        private WaveVR_InputModuleManager waveInputManager;

        [SerializeField]
        private WaveVR_ControllerLoader waveCtrlLoader;
#endif

        public InputInterface platformInput;

        private static InputWrapper _instance;
        public static InputWrapper instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputWrapper>();
                    _instance.SetUp();
                }
                return _instance;
            }
        }

        public void SetUp() {
            standaloneAsset.gameObject.SetActive(false);
            waveAsset.SetActive(false);

#if UNITY_EDITOR
            standaloneAsset.gameObject.SetActive(true);
            platformInput = new InputStandalone(standaloneAsset);
            //waveAsset.SetActive(true);
            //platformInput = new InputWave(waveInputManager);
#elif UNITY_ANDROID
            waveAsset.SetActive(true);
            platformInput = new InputWave(waveInputManager);
#endif
        }
    }
}

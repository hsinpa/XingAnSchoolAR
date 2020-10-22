using Expect.View;
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Expect.ARTour
{
    public class SoundTextTourCtrl : ObserverPattern.Observer
    {
        [SerializeField]
        private Button tourButton;

        [SerializeField]
        private Button updateButton;

        [SerializeField]
        private List<TourGuideData> tourGuideDataList;

        public static System.Action<GeneralFlag.ARTourTheme> OnThemeChange;
        public static System.Action<ARDataSync> OnDataSync;
        public static GeneralFlag.ARTourTheme arTourTheme = GeneralFlag.ARTourTheme.None;

        ARDataSync _arDataSync;
        ARTourModel _model;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            base.OnNotify(p_event, p_objects);

            switch (p_event) {
                case GeneralFlag.ObeserverEvent.TourStart:
                    arTourTheme = GeneralFlag.ARTourTheme.None;
                    break;

                case GeneralFlag.ObeserverEvent.AppEnd:
                    tourButton.gameObject.SetActive(false);
                    break;

                case GeneralFlag.ObeserverEvent.ThemeChange:
                    OnThemeChange((GeneralFlag.ARTourTheme)p_objects[0]);
                    OnARObjectUpdateBtn();
                    break;
            }
        }

        private void Start()
        {
            _arDataSync = new ARDataSync();
            _model = MainApp.Instance.model.GetModel<ARTourModel>();

            tourButton.onClick.AddListener(OnClickTourBtn);
            updateButton.onClick.AddListener(OnARObjectUpdateBtn);

            //Hide by default
            tourButton.gameObject.SetActive(false);
            OnThemeChange += OnThemeChangeCallback;
        }

        private void OnClickTourBtn() {
            if (arTourTheme == GeneralFlag.ARTourTheme.None) return;

            if (GeneralFlag.ThemeKeyLookUpTable.TryGetValue(arTourTheme, out string p_key))
            {

                var guideSRP = tourGuideDataList.Find(x => x._id.Equals(p_key));

                if (guideSRP.isValid) {
                    TourView tourModal = Modals.instance.OpenModal<TourView>();
                    tourModal.SetUp(p_key, _model, guideSRP.GuideBoardSRP, OnQuestionStartClick);
                }
            }

        }

        private void OnQuestionStartClick() {
            if (GeneralFlag.QThemeLookUpTable.TryGetValue(arTourTheme, out string firstQEventKey)) {
                MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.QuizStart, firstQEventKey);
            }
        }

        private void OnARObjectUpdateBtn()
        {
            StartCoroutine(
            _arDataSync.WebSyncARData(() =>
            {
                if (OnDataSync != null)
                    OnDataSync(_arDataSync);
            }));
        }

        private void OnThemeChangeCallback(GeneralFlag.ARTourTheme theme) {
            tourButton.gameObject.SetActive(true);
            arTourTheme = theme;
            
            Debug.Log("Theme is pick " + theme.ToString("g"));
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Spring);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Summer);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Autumn);

            if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Winter);
        }
#endif

        [System.Serializable]
        public struct TourGuideData {
            public GuideBoardSRP GuideBoardSRP;
            public string _id;

            public bool isValid => !string.IsNullOrEmpty(_id);
        }

    }
}
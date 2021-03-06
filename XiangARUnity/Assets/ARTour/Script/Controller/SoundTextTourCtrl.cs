﻿using Expect.View;
using Hsinpa.View;
using Questionaire;
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
        private Image scanImage;

        [SerializeField]
        private List<TourGuideData> tourGuideDataList;

        public static System.Action<GeneralFlag.ARTourTheme> OnThemeChange;
        public static System.Action<ARDataSync> OnDataSync;
        public static GeneralFlag.ARTourTheme arTourTheme = GeneralFlag.ARTourTheme.None;

        ARDataSync _arDataSync;
        ARTourModel _model;
        TourGuideData _tourGuideData;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            base.OnNotify(p_event, p_objects);

            switch (p_event) {
                case GeneralFlag.ObeserverEvent.TourStart:

                    //OnThemeChangeCallback(GeneralFlag.ARTourTheme.None);

                    ShowLibraryIntro();

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
            scanImage.enabled = false;

            OnThemeChange += OnThemeChangeCallback;

            OnThemeChangeCallback(GeneralFlag.ARTourTheme.None);
        }

        private void ShowLibraryIntro()
        {
            var guideSRP = tourGuideDataList.Find(x => x._id.Equals(GeneralFlag.ARTour.TourGuide.LibraryIntro));

            TourView tourModal = Modals.instance.OpenModal<TourView>();
            tourModal.SetUp(GeneralFlag.ARTour.TourGuide.LibraryIntro, _model, guideSRP.GuideBoardSRP, null, ShowLibraryDetail);
        }

        private void ShowLibraryDetail()
        {
            var guideSRP = tourGuideDataList.Find(x => x._id.Equals(GeneralFlag.ARTour.TourGuide.LibraryPaint));

            TourView tourModal = Modals.instance.OpenModal<TourView>();
            tourModal.SetUp(GeneralFlag.ARTour.TourGuide.LibraryPaint, _model, guideSRP.GuideBoardSRP, null, () => {
                if (arTourTheme == GeneralFlag.ARTourTheme.None)
                    scanImage.enabled = true;
            });
        }

        private void OnClickTourBtn() {
            if (arTourTheme == GeneralFlag.ARTourTheme.None) return;

            if (GeneralFlag.ThemeKeyLookUpTable.TryGetValue(arTourTheme, out string p_key))
            {
                _tourGuideData = tourGuideDataList.Find(x => x._id.Equals(p_key));
                 
                if (_tourGuideData.isValid) {
                    TourView tourModal = Modals.instance.OpenModal<TourView>();
                    tourModal.SetUp(p_key, _model, _tourGuideData.GuideBoardSRP, OnQuestionStartClick, null);
                }
            }
        }

        private void OnQuestionStartClick() {
            if (GeneralFlag.QThemeLookUpTable.TryGetValue(arTourTheme, out string firstQEventKey)) {
                MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.QuizStart, firstQEventKey, _tourGuideData.GuideBoardSRP);
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
            arTourTheme = theme;

            tourButton.gameObject.SetActive(false);

            if (theme == GeneralFlag.ARTourTheme.None) return;

            tourButton.gameObject.SetActive(true);
            scanImage.enabled = false;

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

        private void OnDestroy()
        {
            OnThemeChange -= OnThemeChangeCallback;
        }

    }
}
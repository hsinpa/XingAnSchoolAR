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

        public static System.Action<GeneralFlag.ARTourTheme> OnThemeChange;
        public static System.Action<ARDataSync> OnDataSync;

        ARDataSync _arDataSync;
        ARTourModel _model;

        private GeneralFlag.ARTourTheme _arTourTheme = GeneralFlag.ARTourTheme.None;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            base.OnNotify(p_event, p_objects);

            switch (p_event) {
                case GeneralFlag.ObeserverEvent.TourStart:
                    break;

                case GeneralFlag.ObeserverEvent.AppEnd:
                    tourButton.gameObject.SetActive(false);
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
            if (_arTourTheme == GeneralFlag.ARTourTheme.None) return;

            TourView tourModal = Modals.instance.OpenModal<TourView>();
            tourModal.SetUp(_arTourTheme, _model, "Game is foot", OnQuestionStartClick);
        }

        private void OnQuestionStartClick() {
            if (GeneralFlag.QThemeLookUpTable.TryGetValue(_arTourTheme, out string firstQEventKey)) {
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
            _arTourTheme = theme;
            
            Debug.Log("Theme is pick " + theme.ToString("g"));
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Spring);

            if (Input.GetKeyDown(KeyCode.Alpha2) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Summer);

            if (Input.GetKeyDown(KeyCode.Alpha3) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Autumn);

            if (Input.GetKeyDown(KeyCode.Alpha4) && OnThemeChange != null)
                OnThemeChange(GeneralFlag.ARTourTheme.Winter);
        }
#endif

    }
}
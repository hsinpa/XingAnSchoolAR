using Expect.View;
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
        FireStoreUtility _fireStoreUtility;

        ARTourModel _model;

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
                    break;
            }
        }

        private void Start()
        {
            _arDataSync = new ARDataSync();
            _fireStoreUtility = new FireStoreUtility();
            _fireStoreUtility.OnInit += OnFireBaseReady;
            _model = MainApp.Instance.model.GetModel<ARTourModel>();

            tourButton.onClick.AddListener(OnClickTourBtn);

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

                var guideSRP = tourGuideDataList.Find(x => x._id.Equals(p_key));

                if (guideSRP.isValid) {
                    TourView tourModal = Modals.instance.OpenModal<TourView>();
                    tourModal.SetUp(p_key, _model, guideSRP.GuideBoardSRP, OnQuestionStartClick, null);
                }
            }
        }

        private void OnQuestionStartClick() {
            if (GeneralFlag.QThemeLookUpTable.TryGetValue(arTourTheme, out string firstQEventKey)) {
                MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.QuizStart, firstQEventKey);
            }
        }

        private void OnThemeChangeCallback(GeneralFlag.ARTourTheme theme) {
            arTourTheme = theme;

            tourButton.gameObject.SetActive(false);

            if (theme == GeneralFlag.ARTourTheme.None) return;

            tourButton.gameObject.SetActive(true);
            scanImage.enabled = false;

            Debug.Log("Theme is pick " + theme.ToString("g"));
        }

        #region FireBase Region
        private void OnFireBaseReady() {
            Debug.Log("OnFireBaseReady");

            //_ = _fireStoreUtility.GetOnceCollection(GeneralFlag.FireBase.AnimalARCol);
            _fireStoreUtility.ListenToCollection(GeneralFlag.FireBase.AnimalARCol);
            _fireStoreUtility.OnFireBaseDocEvent += OnFireStoreEvent;
        }

        private void OnFireStoreEvent(List<Firebase.Firestore.DocumentSnapshot> documentSnapshot) {

            for (int i = 0; i < documentSnapshot.Count; i++) {
                if (documentSnapshot[i].Exists)
                    _arDataSync.SaveFireStoreData(documentSnapshot[i].ToDictionary());
            }

            if (OnDataSync != null)
                OnDataSync(_arDataSync);
        }
             
        #endregion

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
            _fireStoreUtility.OnFireBaseDocEvent -= OnFireStoreEvent;

            if (_fireStoreUtility != null)
                _fireStoreUtility.UnRegisterAll();
        }

    }
}
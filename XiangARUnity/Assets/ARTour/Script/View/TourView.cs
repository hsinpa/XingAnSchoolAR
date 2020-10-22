using System.Collections;
using System.Collections.Generic;
using Expect.StaticAsset;
using Hsinpa.View;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class TourView : Modal
    {
        [Header("Content")]

        [SerializeField]
        private Text title;

        [SerializeField]
        private Text contentText;

        [SerializeField]
        private RectTransform ContentScrollRect;

        [SerializeField]
        private RectTransform ViewportScrollRect;

        [SerializeField]
        private Button leftBtn;

        [SerializeField]
        private Button rightBtn;

        [Header("Voice")]

        [SerializeField]
        private Button engVoiceBtn;

        [SerializeField]
        private Button chtVoiceBtn;

        [Header("Rest")]

        [SerializeField]
        private Button startQuestionaireBtn;

        [SerializeField]
        private Button closeBtn;

        private ARTourModel _model;

        private string _themeKey;

        private System.Action OnQuestionStartCallback;

        private void Start()
        {
            startQuestionaireBtn.onClick.AddListener(OnQuestionaireClick);

            closeBtn.onClick.AddListener(() =>
            {
                Modals.instance.Close();
            });
        }

        public void SetUp(string tour_id, ARTourModel model, GuideBoardSRP guideBoardSRP, System.Action Callback) {
            this._model = model;

            title.text = guideBoardSRP.title;
            contentText.text = guideBoardSRP.textAsset.text;

            OnQuestionStartCallback = Callback;

            //Check question is being take or not
            int questionRecord = _model.GetVariable(tour_id);
            startQuestionaireBtn.gameObject.SetActive(questionRecord == 0 && Callback != null);
        }

        private void OnQuestionaireClick() {
            Modals.instance.Close();

            if (OnQuestionStartCallback != null)
                OnQuestionStartCallback();
        }
    }
}
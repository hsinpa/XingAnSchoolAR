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
        private ScrollRect ScrollRect;

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

        private System.Action OnQuestionStartCallback;

        private float viewportHeight, contentHeight;
        private float currentHeight => ContentScrollRect.anchoredPosition.y;
        private float maxHeight => contentHeight - viewportHeight;

        private void Start()
        {
            startQuestionaireBtn.onClick.AddListener(OnQuestionaireClick);

            closeBtn.onClick.AddListener(() =>
            {
                Modals.instance.Close();
            });

            leftBtn.onClick.AddListener(OnLeftBtnClick);
            rightBtn.onClick.AddListener(OnRightBtnClick);
            ScrollRect.onValueChanged.AddListener(OnScrollViewChange);
        }

        public void SetUp(string tour_id, ARTourModel model, GuideBoardSRP guideBoardSRP, System.Action Callback) {
            this._model = model;

            title.text = guideBoardSRP.title;
            contentText.text = guideBoardSRP.textAsset.text;

            OnQuestionStartCallback = Callback;

            //Check question is being take or not
            int questionRecord = _model.GetVariable(tour_id);
            startQuestionaireBtn.gameObject.SetActive(questionRecord == 0 && Callback != null);

            StartCoroutine(ConfigTextContent());
        }

        private IEnumerator ConfigTextContent() {
            yield return new WaitForEndOfFrame();
            ContentScrollRect.anchoredPosition = Vector2.zero;

            viewportHeight = ViewportScrollRect.rect.height;
            contentHeight = ContentScrollRect.sizeDelta.y;
            SetLeftRightContentBtn();
        }

        private void SetLeftRightContentBtn() {
            leftBtn.gameObject.SetActive(currentHeight > 0);
            rightBtn.gameObject.SetActive(currentHeight < (maxHeight));
        }

        private void OnLeftBtnClick() {
            ProcessBtnClickAction(-1);
        }

        private void OnRightBtnClick()
        {
            ProcessBtnClickAction(1);
        }

        private void ProcessBtnClickAction(int dir) {
            float _currentHeight = currentHeight + viewportHeight * dir;
            _currentHeight = Mathf.Clamp(_currentHeight, 0, maxHeight);

            ContentScrollRect.anchoredPosition = new Vector2(0, _currentHeight);
            SetLeftRightContentBtn();
        }

        private void OnScrollViewChange(Vector2 pos) {
            SetLeftRightContentBtn();
        }

        private void OnQuestionaireClick() {
            Modals.instance.Close();

            if (OnQuestionStartCallback != null)
                OnQuestionStartCallback();
        }
    }
}
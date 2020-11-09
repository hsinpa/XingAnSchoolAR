using System.Collections;
using System.Collections.Generic;
using Expect.StaticAsset;
using Hsinpa.View;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;

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
        private Text startQuestionaireBtnText;

        [SerializeField]
        private Button closeBtn;

        private ARTourModel _model;

        private System.Action OnQuestionStartCallback;
        private System.Action OnCloseBtnCallback;

        private GuideBoardSRP _guideBoardSRP;
        private float viewportHeight, contentHeight;
        private float currentHeight => ContentScrollRect.anchoredPosition.y;
        private float maxHeight => contentHeight - viewportHeight;

        private void Start()
        {
            startQuestionaireBtn.onClick.AddListener(OnQuestionaireClick);

            closeBtn.onClick.AddListener(OnCloseBtnClick);
            leftBtn.onClick.AddListener(OnLeftBtnClick);
            rightBtn.onClick.AddListener(OnRightBtnClick);
            ScrollRect.onValueChanged.AddListener(OnScrollViewChange);

            chtVoiceBtn.onClick.AddListener(() => PlayTourAudio(_guideBoardSRP.chtAudioGuide, _guideBoardSRP.textAsset));
            engVoiceBtn.onClick.AddListener(() => PlayTourAudio(_guideBoardSRP.enAudioGuide, _guideBoardSRP.textAsset_en));
        }

        public void SetUp(string tour_id, ARTourModel model, GuideBoardSRP guideBoardSRP, System.Action questionBtnCallback, System.Action closeBtnCallback) {
            this._model = model;
            this._guideBoardSRP = guideBoardSRP;

            title.text = guideBoardSRP.title;
            startQuestionaireBtnText.text = guideBoardSRP.btnName;

            AssignContentText(guideBoardSRP.textAsset);

            OnQuestionStartCallback = questionBtnCallback;
            OnCloseBtnCallback = closeBtnCallback;

            //Check question is being take or not
            int questionRecord = _model.GetVariable(tour_id);
            startQuestionaireBtn.gameObject.SetActive(questionRecord == 0);

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

        private void PlayTourAudio(AudioClip clip, TextAsset textAsset) {
            if (clip != null)
                UniversalAudioSolution.instance.PlayAudio(UniversalAudioSolution.AudioType.AudioClip2D, clip);

            AssignContentText(textAsset);
        }

        private void AssignContentText(TextAsset textAsset) {
            if (textAsset != null)
            {
                contentText.text = textAsset.text;
                contentText.text = contentText.text.Replace("\\n", "\n");
            }
        }

        private void OnCloseBtnClick() {
            Modals.instance.Close();

            UniversalAudioSolution.instance.StopAudio(UniversalAudioSolution.AudioType.AudioClip2D);

            if (OnCloseBtnCallback != null)
                OnCloseBtnCallback();
        }

        private void OnQuestionaireClick() {

            if (OnQuestionStartCallback == null) {
                OnCloseBtnClick();
                return;
            }

            Modals.instance.Close();
            UniversalAudioSolution.instance.StopAudio(UniversalAudioSolution.AudioType.AudioClip2D);

            if (OnQuestionStartCallback != null)
                OnQuestionStartCallback();
        }
    }
}
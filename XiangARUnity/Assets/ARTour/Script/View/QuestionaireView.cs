using Expect.StaticAsset;
using Hsinpa.View;
using Questionaire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hsinpa.Utility;

namespace Expect.View
{
    public class QuestionaireView : Modal
    {
        [SerializeField]
        private GameObject AnswerSlotPrefab;

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Text questionText;

        [SerializeField]
        private Text scoreText;

        [SerializeField]
        private Transform AnswerContainer;

        [SerializeField]
        private Button ProceedBtn;
        private Text ProceedBtnText;

        [Header("Hint Group")]
        [SerializeField]
        private Image hintImage;

        [SerializeField]
        private Sprite boolChoiceSprite;

        [SerializeField]
        private Sprite multipleChoiceSprite;

        [SerializeField]
        private Text resultHintText;

        [Header("True / False Group")]
        [SerializeField]
        private RectTransform truefalseGroup;

        [SerializeField]
        private Button trueGroupBtn;
        private Image trueGroupBtnImage => trueGroupBtn.GetComponent<Image>();

        [SerializeField]
        private Button falseGroupBtn;
        private Image falseGroupBtnImage => falseGroupBtn.GetComponent<Image>();

        private int currentIndex = -1;
        private int correctAnswerIndex = -1;

        private string[] _rawAnswerSlots;
        private AnswerSloItem[] _answerSlotItems;

        private System.Action<int, bool> OnProceedClickEvent;

        private bool isCorrectAnswerDisplay = false;

        private void Start()
        {
            ProceedBtn.onClick.AddListener(OnProceedBtnClick);
            ProceedBtnText = ProceedBtn.GetComponentInChildren<Text>();
        }

        public void SetContent(string title, string question, string[] answers, int correctAnswerIndex, System.Action<int, bool> OnProceedCallback) {
            currentIndex = -1;
            this.correctAnswerIndex = correctAnswerIndex;

            _rawAnswerSlots = answers;

            titleText.text = title;

            questionText.text = question;

            scoreText.text = "";

            resultHintText.text = "";

            bool isMultitpleChoise = answers.Length > 2;

            if (isMultitpleChoise)
            {
                CreateAnswerSlots(answers);
            }
            else {
                CreateTrueFalseGroup();
            }

            hintImage.sprite = (isMultitpleChoise) ? multipleChoiceSprite : boolChoiceSprite;

            ProceedBtn.interactable = false;

            isCorrectAnswerDisplay = false;

            OnProceedClickEvent = OnProceedCallback;

            ProceedBtnText.text = StringAsset.ARTour.QuestionaireSubmitBtn;
        }

        #region TrueFalse Group
        private void OnTrueFalseClick(Button btn, int index) {
            if (isCorrectAnswerDisplay) return;

            currentIndex = index;

            Button otherBtn = (btn == falseGroupBtn) ? trueGroupBtn : falseGroupBtn;
            Image otherBtnImage = otherBtn.GetComponent<Image>();
            Image mainBtnImage = btn.GetComponent<Image>();

            otherBtnImage.color = Color.white;
            mainBtnImage.color = Color.gray;

            ProceedBtn.interactable = true;
        }

        private void CreateTrueFalseGroup() {
            AnswerContainer.gameObject.SetActive(false);
            truefalseGroup.gameObject.SetActive(true);

            trueGroupBtn.interactable = true;
            falseGroupBtn.interactable = true;

            trueGroupBtnImage.color = Color.white;
            falseGroupBtnImage.color = Color.white;

            trueGroupBtn.onClick.RemoveAllListeners();
            falseGroupBtn.onClick.RemoveAllListeners();

            trueGroupBtn.onClick.AddListener(() =>
            {
                OnTrueFalseClick(trueGroupBtn, 0);
            });

            falseGroupBtn.onClick.AddListener(() =>
            {
                OnTrueFalseClick(falseGroupBtn, 1);
            });
        }
        #endregion

        private void CreateAnswerSlots(string[] answers) {
            AnswerContainer.gameObject.SetActive(true);
            truefalseGroup.gameObject.SetActive(false);

            UtilityMethod.ClearChildObject(AnswerContainer);

            _answerSlotItems = new AnswerSloItem[answers.Length];

            for (int i = 0; i < answers.Length; i++) {
                var slotitem = UtilityMethod.CreateObjectToParent(AnswerContainer, AnswerSlotPrefab).GetComponent<AnswerSloItem>();
                _answerSlotItems[i] = slotitem;

                slotitem.SetContent(answers[i], HighlightCurrentSelectSlot);
            }
        }

        private void HighlightCurrentSelectSlot(AnswerSloItem p_item) {
            if (_answerSlotItems == null || p_item == null) return;

            ProceedBtn.interactable = true;

            for (int i = 0; i < _answerSlotItems.Length; i++) {
                AnswerSloItem item = _answerSlotItems[i];

                bool isCurrentSelected = item == p_item;
                item.SetTickStatus(isCurrentSelected);

                if (isCurrentSelected)
                    currentIndex = i;
            }
        }

        private void DisplayCorrectAnswerLayout(int correctIndex) {
            if (correctIndex < 0) return;

            if (_answerSlotItems != null) {
                for (int i = 0; i < _answerSlotItems.Length; i++)
                {
                    _answerSlotItems[i].EnableBoolImg(false);
                    _answerSlotItems[i].SetState(AnswerSloItem.State.Wrong);
                }

                _answerSlotItems[correctIndex].EnableBoolImg(true);
                _answerSlotItems[correctIndex].SetState(AnswerSloItem.State.Correct);
            }

            ProceedBtnText.text = StringAsset.ARTour.QuestionaireContinueBtn;

            bool isCorrect = currentIndex == correctIndex;

            resultHintText.color = (isCorrect) ? ParameterFlag.Style.Correct : ParameterFlag.Style.Wrong;
            resultHintText.text = (isCorrect) ? StringAsset.ARTour.QuestionarieCorrectMsg : StringAsset.ARTour.QuestionarieWrongMsg;

            trueGroupBtnImage.color = (0 == correctIndex) ? Color.white : Color.gray;
            falseGroupBtnImage.color = (1 == correctIndex) ? Color.white : Color.gray;
        }

        private void OnProceedBtnClick() {

            if (!isCorrectAnswerDisplay) {
                DisplayCorrectAnswerLayout(this.correctAnswerIndex);
                isCorrectAnswerDisplay = true;
                return;
            }

            if (OnProceedClickEvent != null)
                OnProceedClickEvent (currentIndex, currentIndex == correctAnswerIndex);
        }

    }
}
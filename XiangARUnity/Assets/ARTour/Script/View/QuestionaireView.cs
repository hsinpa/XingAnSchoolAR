using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        private Transform AnswerContainer;

        [SerializeField]
        private Button ProceedBtn;

        private int currentIndex = -1;
        private string[] _rawAnswerSlots;
        private AnswerSloItem[] _answerSlotItems;

        private System.Action<int> OnProceedClickEvent;

        private void Start()
        {
            ProceedBtn.onClick.AddListener(OnProceedBtnClick);
        }

        public void SetContent(string title, string question, string[] answers, int correctAnswerIndex, System.Action<int> OnProceedCallback) {
            currentIndex = -1;

            _rawAnswerSlots = answers;

            titleText.text = title;

            questionText.text = question;

            CreateAnswerSlots(answers);

            ProceedBtn.interactable = false;

            OnProceedClickEvent = OnProceedCallback;
        }

        private void CreateAnswerSlots(string[] answers) {
            Utility.UtilityMethod.ClearChildObject(AnswerContainer);
            _answerSlotItems = new AnswerSloItem[answers.Length];
            for (int i = 0; i < answers.Length; i++) {
                var slotitem = Utility.UtilityMethod.CreateObjectToParent(AnswerContainer, AnswerSlotPrefab).GetComponent<AnswerSloItem>();
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

        private void OnProceedBtnClick() {

            if (OnProceedClickEvent != null)
                OnProceedClickEvent(currentIndex);
        }

    }
}
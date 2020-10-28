﻿using Expect.StaticAsset;
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

        private int currentIndex = -1;
        private int correctAnswerIndex = -1;

        private string[] _rawAnswerSlots;
        private AnswerSloItem[] _answerSlotItems;

        private System.Action<int> OnProceedClickEvent;

        private bool isCorrectAnswerDisplay = false;

        private void Start()
        {
            ProceedBtn.onClick.AddListener(OnProceedBtnClick);
            ProceedBtnText = ProceedBtn.GetComponentInChildren<Text>();
        }

        public void SetContent(string title, string question, string[] answers, int correctAnswerIndex, System.Action<int> OnProceedCallback) {
            currentIndex = -1;
            this.correctAnswerIndex = correctAnswerIndex;

            _rawAnswerSlots = answers;

            titleText.text = title;

            questionText.text = question;

            scoreText.text = "";

            CreateAnswerSlots(answers);

            ProceedBtn.interactable = false;

            isCorrectAnswerDisplay = false;

            OnProceedClickEvent = OnProceedCallback;

            ProceedBtnText.text = StringAsset.ARTour.QuestionaireSubmitBtn;
        }

        private void CreateAnswerSlots(string[] answers) {
            UtilityMethod.ClearChildObject(AnswerContainer);
            _answerSlotItems = new AnswerSloItem[answers.Length];
            for (int i = 0; i < answers.Length; i++) {
                var slotitem = UtilityMethod.CreateObjectToParent(AnswerContainer, AnswerSlotPrefab).GetComponent<AnswerSloItem>();
                _answerSlotItems[i] = slotitem;


                slotitem.SetContent(answers[i], HighlightCurrentSelectSlot);

                if (answers[i] == StringAsset.ARTour.ConfirmText)
                    slotitem.EnableBoolImg(true);

                if (answers[i] == StringAsset.ARTour.MistakeText)
                    slotitem.EnableBoolImg(false);
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
            if (_answerSlotItems == null || correctIndex < 0) return;

            for (int i = 0; i < _answerSlotItems.Length; i++)
                _answerSlotItems[i].SetState(AnswerSloItem.State.Wrong);

            _answerSlotItems[correctIndex].SetState(AnswerSloItem.State.Correct);

            ProceedBtnText.text = StringAsset.ARTour.QuestionaireContinueBtn;

            if (currentIndex == correctIndex)
                scoreText.text = string.Format(StringAsset.ARTour.AddScoreText, 25);
        }

        private void OnProceedBtnClick() {

            if (!isCorrectAnswerDisplay) {
                DisplayCorrectAnswerLayout(this.correctAnswerIndex);
                isCorrectAnswerDisplay = true;
                return;
            }

            if (OnProceedClickEvent != null)
                OnProceedClickEvent (currentIndex);
        }

    }
}
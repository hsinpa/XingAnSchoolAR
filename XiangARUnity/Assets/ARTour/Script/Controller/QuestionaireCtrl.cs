using Expect.View;
using Hsinpa.View;
using Questionaire;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Expect.ARTour
{
    public class QuestionaireCtrl : ObserverPattern.Observer
    {
        private ARTourModel _model;

        private QuestionaireView _questionaireView;

        private Ticket currentTicket;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            base.OnNotify(p_event, p_objects);

            switch (p_event)
            {
                case GeneralFlag.ObeserverEvent.QuizStart:

                    StartQuiz((string)p_objects[0]);
                    break;
            }
        }

        private void StartQuiz(string firstQ_key) {
            _model = MainApp.Instance.model.GetModel<ARTourModel>();

            currentTicket = _model.EnterQuestionaire(firstQ_key);

            _questionaireView = Modals.instance.OpenModal<QuestionaireView>();

            string debugMsg = string.Format("ID : {0}, Value : {1}", currentTicket.eventStats._ID, currentTicket.eventStats.MainValue);
            Debug.Log(debugMsg);

            ProcessTicket(currentTicket);
        }

        private void SetQuestionToView(Ticket ticket) {
            string keyValue = "score + 1";
            int correctIndex = ticket.choiceStats.FindIndex(x => x.Effect.IndexOf(keyValue) >= 0);

            if (correctIndex < 0) {
                Debug.LogError(ticket.eventStats._ID +", has no correct answer");
                return;
            }

            string[] potentialAnswers = ticket.choiceStats.Select(x => x.MainValue).ToArray<string>();

            _questionaireView.SetContent("低年級", ticket.eventStats.MainValue, potentialAnswers, correctIndex, OnAnswerSubmit);
        }

        private void ProcessTicket(Ticket ticket) {

            if (ticket.eventStats.Tag == GeneralFlag.ARTour.QuestionType.End)
            {
                //TODO: Show Score Board;

                return;
            }

            if (ticket.eventStats.Tag == GeneralFlag.ARTour.QuestionType.Continue) {
                Modals.instance.Close();
                return;
            }

            SetQuestionToView(ticket);
        }


        private void OnAnswerSubmit(int selectedIndex)
        {
            ProcessTicket(_model.SubmitChoiceWithIndex(currentTicket, selectedIndex));
        }

    }
}
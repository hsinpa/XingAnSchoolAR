using Expect.View;
using Hsinpa.View;
using Questionaire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Expect.ARTour
{
    public class QuestionaireCtrl : ObserverPattern.Observer
    {
        private ARTourModel _model;

        private QuestionaireView _questionaireView;

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

            Ticket ticket = _model.EnterQuestionaire(firstQ_key);

            _questionaireView = Modals.instance.OpenModal<QuestionaireView>();

            string debugMsg = string.Format("ID : {0}, Value : {1}", ticket.eventStats._ID, ticket.eventStats.MainValue);
            Debug.Log(debugMsg);
        }

        private void ProcessTicket(Ticket ticket) {

            if (ticket.eventStats.Tag == GeneralFlag.ARTour.QuestionType.End)
            {
                //TODO: Show Score Board;

                return;
            }

            if (ticket.eventStats.Tag == GeneralFlag.ARTour.QuestionType.Continue) {

            }
        }



    }
}
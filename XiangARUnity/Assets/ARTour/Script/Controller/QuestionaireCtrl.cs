using Expect.View;
using Hsinpa.View;
using Questionaire;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Expect.StaticAsset;
using UnityEngine.SceneManagement;

namespace Expect.ARTour
{
    public class QuestionaireCtrl : ObserverPattern.Observer
    {
        private ARTourModel _model;

        private QuestionaireView _questionaireView;

        private Ticket currentTicket;
        private GuideBoardSRP _guideBoardSRP;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            base.OnNotify(p_event, p_objects);

            switch (p_event)
            {
                case GeneralFlag.ObeserverEvent.QuizStart:
                    _guideBoardSRP = (GuideBoardSRP)p_objects[1];
                    StartQuiz((string)p_objects[0]);
                    break;
            }
        }

        private void StartQuiz(string firstQ_key) {
            _model = MainApp.Instance.model.GetModel<ARTourModel>();

            currentTicket = _model.EnterQuestionaire(firstQ_key);

            _questionaireView = Modals.instance.OpenModal<QuestionaireView>();

            string debugMsg = string.Format("ID : {0}, Value : {1}", currentTicket.eventStats._ID, currentTicket.eventStats.MainValue);

            ProcessTicket(currentTicket, false);
        }

        private void SetQuestionToView(Ticket ticket) {
            string keyValue = "score + 1";
            int correctIndex = ticket.choiceStats.FindIndex(x => x.Effect.IndexOf(keyValue) >= 0);

            Debug.Log("correctIndex " + correctIndex);

            if (correctIndex < 0) {
                Debug.LogError(ticket.eventStats._ID +", has no correct answer");
                return;
            }

            string[] potentialAnswers = ticket.choiceStats.Select(x => x.MainValue).ToArray<string>();
            string title = StringAsset.GetGradeString(PlayerPrefs.GetInt(GeneralFlag.Playerpref.Level, 0));

            _questionaireView.SetContent(title, ticket.eventStats.MainValue, potentialAnswers, correctIndex, OnAnswerSubmit);
        }

        private void ProcessTicket(Ticket ticket, bool isCorrect) {

            if (ticket.eventStats.Tag == GeneralFlag.ARTour.QuestionType.Continue)
            {
                ShowMidScorePage(isCorrect, null);

                return;
            }

            if (ticket.eventStats.Tag == GeneralFlag.ARTour.QuestionType.End)
            {
                ShowMidScorePage(isCorrect, () => {
                    ShowFinalScorePage();
                });

                return;
            }

            SetQuestionToView(ticket);
        }

        private void ShowMidScorePage(bool isCorrect, System.Action p_callback) {
            Modals.instance.Close();
            int score = (!isCorrect) ? 0 : 25;
            ScoreView scoreView = Modals.instance.OpenModal<ScoreView>();

            scoreView.SetContent(_guideBoardSRP.title, StringAsset.ARTour.ShowAreaScoreStatement, "",
            $"+{ score }分", StringAsset.ARTour.ShowAreaScoreButton, () => {
                Modals.instance.Close();

                if (p_callback != null) {
                    p_callback();
                }
            });
        }

        private void ShowFinalScorePage() {
            Modals.instance.Close();
            int score = _model.GetVariable(ParameterFlag.QuestionaireParameter.Score) * 25;

            ScoreView scoreView = Modals.instance.OpenModal<ScoreView>();
            scoreView.SetContent(StringAsset.ARTour.EndGameTitle, StringAsset.ARTour.EndGameStatement, StringAsset.ARTour.EndGameSubStatement,
                $"+{score}分", StringAsset.ARTour.EndGameButton, () => {
                    _model.Reset();
                    SceneManager.LoadScene("ARTour");
                });
        }


        private void OnAnswerSubmit(int selectedIndex, bool isCorrect)
        {
            ProcessTicket(_model.SubmitChoiceWithIndex(currentTicket, selectedIndex), isCorrect);
        }

    }
}
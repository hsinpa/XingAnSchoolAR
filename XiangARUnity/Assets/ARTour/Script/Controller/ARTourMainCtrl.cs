using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hsinpa.View;
using Expect.View;
using Expect.StaticAsset;
using UnityEngine.XR.ARFoundation;

namespace Expect.ARTour {
    public class ARTourMainCtrl : ObserverPattern.Observer
    {

        [SerializeField]
        ARSession ARSession;

        public static GeneralFlag.Language _language;

        public override void OnNotify(string p_event, params object[] p_objects)
        {
            base.OnNotify(p_event, p_objects);

            switch (p_event) {
                case GeneralFlag.ObeserverEvent.AppStart:
                    Debug.Log("App Start");
                    ResetData();
                    StartWithSelectLang();

                    break;

                case GeneralFlag.ObeserverEvent.AppEnd:

                    break;
            }
        }

        private void StartWithSelectLang() {
            DialogueModal dialogueModal = Modals.instance.OpenModal<DialogueModal>();
            string[] levels = new string[] { StringAsset.ARTour.LowGrade, StringAsset.ARTour.MiddleGrade, StringAsset.ARTour.HighGrade };

            dialogueModal.SetDialogue(StringAsset.ARTour.ChooseGradeTitle, StringAsset.ARTour.ChooseGradeContent,
                levels,
                (int p_index) =>
                {
                    PlayerPrefs.SetInt(GeneralFlag.Playerpref.Level, p_index);

                    StartFreeTour();
                }
            );
        }

        private void StartFreeTour() {
            //ARSession.enabled = true;
            MainApp.Instance.Notify(GeneralFlag.ObeserverEvent.TourStart);
        }

        private void ShowTotalScore() {
            ScoreView scoreView = Modals.instance.OpenModal<ScoreView>();



        }

        private void ResetData() {
            //ARSession.enabled = false;
        }

    }
}
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
            }
        }

        private void StartWithSelectLang() {
            DialogueModal dialogueModal = Modals.instance.OpenModal<DialogueModal>();
            dialogueModal.SetDialogue(StringAsset.ARTour.ChooseLangTitle, StringAsset.ARTour.ChooseLangContent, new string[] { StringAsset.ARTour.LangEN, StringAsset.ARTour.LangCH },
                (string p_lang) =>
                {
                    _language = (p_lang == StringAsset.ARTour.LangEN) ? GeneralFlag.Language.En : GeneralFlag.Language.Ch;

                }
            );
        }

        private void StartFreeTour() {
            ARSession.enabled = true;
        }

        private void ResetData() {
            ARSession.enabled = false;
        }

    }
}
using Expect.StaticAsset;
using Hsinpa.View;
using Questionaire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class ScoreView : Modal
    {

        [SerializeField]
        private Text TitleStatment;

        [SerializeField]
        private Text ScoreText;

        [SerializeField]
        private Button ConfirmBtn;


        public void SetContent(string score, System.Action confirmCallback) {
            ScoreText.text = score;

            ConfirmBtn.onClick.RemoveAllListeners();
            ConfirmBtn.onClick.AddListener(() => confirmCallback());
        }

    }
}
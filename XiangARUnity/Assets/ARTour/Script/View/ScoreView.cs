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
        private Text Title;

        [SerializeField]
        private Text TitleStatement;

        [SerializeField]
        private Text ScoreText;

        [SerializeField]
        private Text SubStatement;

        [SerializeField]
        private Button ConfirmBtn;

        public void SetContent(string title, string mainStatement, string subStatment, string score, string btnName, System.Action confirmCallback) {
            Title.text = title;
            ScoreText.text = score;

            SubStatement.gameObject.SetActive(string.IsNullOrEmpty(subStatment) );
            if (subStatment != null)
                SubStatement.text = subStatment;

            TitleStatement.text = mainStatement;

            Text ConfirmBtnText = ConfirmBtn.GetComponentInChildren<Text>();
            ConfirmBtnText.text = btnName;

            ConfirmBtn.onClick.RemoveAllListeners();
            ConfirmBtn.onClick.AddListener(() => confirmCallback());
        }



    }
}
using System.Collections;
using System.Collections.Generic;
using Hsinpa.View;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class TourView : Modal
    {
        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Button startQuestionaireBtn;

        private ARTourModel _model;

        private string _themeKey;

        private System.Action OnQuestionStartCallback;

        private void Start()
        {
            startQuestionaireBtn.onClick.AddListener(OnQuestionaireClick);
        }

        public void SetUp(GeneralFlag.ARTourTheme theme, ARTourModel model, string title, System.Action Callback) {
            this._model = model;
            titleText.text = title;

            if (GeneralFlag.ThemeKeyLookUpTable.TryGetValue(theme, out string p_key)) {
                _themeKey = p_key;
            }

            OnQuestionStartCallback = Callback;

            //Check question is being take or not
            int questionRecord = _model.GetVariable(_themeKey);
            startQuestionaireBtn.interactable = questionRecord == 0;
        }

        private void OnQuestionaireClick() {
            if (OnQuestionStartCallback != null)
                OnQuestionStartCallback();
        }
    }
}
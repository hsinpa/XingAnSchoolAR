using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class AnswerSloItem : MonoBehaviour
    {
        [SerializeField]
        private Image tickImage;

        [SerializeField]
        private Text answerText;

        [SerializeField]
        private Button button;

        public void SetContent(string p_text, System.Action<AnswerSloItem> clickEvent) {
            answerText.text = p_text;
            button.onClick.AddListener(() => clickEvent(this));
        }

        public void SetTickStatus(bool p_enable) {
            tickImage.enabled = p_enable;
        }
    }
}
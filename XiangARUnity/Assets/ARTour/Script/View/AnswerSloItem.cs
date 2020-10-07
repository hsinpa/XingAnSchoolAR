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

        [SerializeField]
        private List<StatePair> state;

        private Image _background;

        public enum State { Normal, Correct, Wrong};

        public void SetContent(string p_text, System.Action<AnswerSloItem> clickEvent) {
            _background = GetComponent<Image>();
            answerText.text = p_text;
            button.onClick.AddListener(() => clickEvent(this));
            SetState(State.Normal);
        }

        public void SetTickStatus(bool p_enable) {
            tickImage.enabled = p_enable;
        }

        public void SetState(State p_state) {
            _background.color = state.Find(x => x.state == p_state).color;

            button.interactable = (p_state == State.Normal);
        }

        [System.Serializable]
        public struct StatePair {
            public State state;
            public Color color;
        }
    }
}
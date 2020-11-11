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

        [Header("Radio")]
        [SerializeField]
        private Sprite checkMark;

        [SerializeField]
        private Sprite errorMark;

        [SerializeField]
        private Image radioMaker;

        private Image _background;

        public enum State { Normal, Correct, Wrong};

        public void SetContent(string p_text, System.Action<AnswerSloItem> clickEvent) {
            radioMaker.enabled = false;
            _background = GetComponent<Image>();
            answerText.text = p_text;
            button.onClick.AddListener(() => clickEvent(this));
            SetState(State.Normal);
        }

        public void EnableBoolImg(bool isCorrect) {
            radioMaker.enabled = true;
            //answerText.enabled = false;

            radioMaker.sprite = (isCorrect) ? checkMark : errorMark;
        }

        public void SetTickStatus(bool p_enable) {
            tickImage.enabled = p_enable;
        }

        public void SetState(State p_state) {
            answerText.color = state.Find(x => x.state == p_state).color;

            button.interactable = (p_state == State.Normal);
        }

        [System.Serializable]
        public struct StatePair {
            public State state;
            public Color color;
        }
    }
}
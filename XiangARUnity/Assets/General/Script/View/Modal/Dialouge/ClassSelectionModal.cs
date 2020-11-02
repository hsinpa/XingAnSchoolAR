using Hsinpa.View;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class ClassSelectionModal : Modal
    {
        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Button levelOneBtn;

        [SerializeField]
        private Button leveTwoBtn;

        [SerializeField]
        private Button levelThreeBtn;

        private System.Action<int> OnBtnClickEvent;

        public void SetCallback(string title, System.Action<int> btnEvent)
        {
            titleText.text = title;
            OnBtnClickEvent = btnEvent;
        }

        private void Start() {
            SetBtnEvent(levelOneBtn, 0);
            SetBtnEvent(leveTwoBtn, 1);
            SetBtnEvent(levelThreeBtn, 2);
        }

        private void SetBtnEvent(Button btn, int index) {
            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() =>
            {
                Modals.instance.Close();

                if (this.OnBtnClickEvent != null)
                    this.OnBtnClickEvent(index);
            });
        }
    }
}
using Hsinpa.View;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Expect.View
{
    public class DialogueModal : Modal
    {

        [SerializeField]
        private Text titleText;

        [SerializeField]
        private Text contentText;

        [SerializeField]
        private Transform buttonContainer;

        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private Image decorateImage;

        private void Start()
        {
        }

        public void DecorateSideImage(Sprite sprite) {
            decorateImage.gameObject.SetActive(sprite != null);
            decorateImage.sprite = sprite;
        }

        public void SetDialogue(string title, string content, string[] allowBtns, System.Action<string> btnEvent) {
            ResetContent();

            titleText.text = title;
            contentText.text = content;

            RegisterButtons(allowBtns, btnEvent);
        }


        private void RegisterButtons(string[] allowBtns, System.Action<string> btnEvent) {
            int btnlength = allowBtns.Length;

            for (int i = 0; i < btnlength; i++) {
                int index = i;
                GameObject buttonObj = Utility.UtilityMethod.CreateObjectToParent(buttonContainer, buttonPrefab);
                Button button = buttonObj.GetComponent<Button>();
                Text textObj = button.GetComponentInChildren<Text>();

                textObj.text = allowBtns[i];

                button.onClick.AddListener(() =>
                {
                    Modals.instance.Close();
                    btnEvent(allowBtns[index]);
                });
            }
        }

        private void ResetContent() {
            DecorateSideImage(null);

            Utility.UtilityMethod.ClearChildObject(buttonContainer);
        }


    }
}
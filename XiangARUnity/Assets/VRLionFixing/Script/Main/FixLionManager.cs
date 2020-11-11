using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.Input;
using Expect.View;
using Expect.StaticAsset;
using Hsinpa.App;
using Hsinpa.Utility;
using Hsinpa.Input;
using Hsinpa.View;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Expect.App {
    public class FixLionManager : MonoBehaviour
    {
        [SerializeField]
        FixLionInput FixLionInput;

        [SerializeField]
        PaintingManager PaintingManager;

        [Header("Lighting")]
        [SerializeField]
        InputWrapper inputWrapper;

        [SerializeField]
        GameObject targetGameObject;

        [SerializeField]
        GameObject lightingObject;

        [Header("UI Components")]
        [SerializeField]
        private Button GameStartBtn;

        [Header("Timeline Components")]
        [SerializeField]
        private PlayableDirector timeline;

        [SerializeField]
        private TimelineAsset EndingTimeAsset;

        [SerializeField]
        private TimelineAsset StartTimeAsset;

        [SerializeField]
        private bool skipAnim;

        private Camera _camera;

        private int progress = 0;
        private int maxProgress = 3;
        private ToolItem currentHoldItem = null;

        // Start is called before the first frame update
        void Start()
        {
            _camera = Camera.main;
            FixLionInput.SetUp();
            FixLionInput.HoldItemEvent += OnTouchToolEvent;
            PaintingManager.OnTargetDirtIsClear += OnDirtIsCleared;

            GameStartBtn.onClick.AddListener(OnStartBtnClick);
        }

        // Update is called once per frame
        void Update()
        {
            FixLionInput.OnUpdate();
            UpdateLightDirAccordingToTarget();
        }

        private void DisplayAfterCleanTourGuide() {
            Debug.LogError("Clean all done");

            timeline.playableAsset = EndingTimeAsset;
            timeline.Play();
        }

        private void OnTouchToolEvent(ToolItem tool) {
            if (currentHoldItem != null) return;

            StringAsset.GetToolStatusType statusType = FixLionUtility.IsGivenToolAllowToProceed(tool.name, this.progress);

            if (statusType != StringAsset.GetToolStatusType.Available) {

                string unavailableMsg = (statusType == StringAsset.GetToolStatusType.AlreadyUsed) ? StringAsset.LionRepairing.ToolUsedMessage : UtilityMethod.GetFromDict<string>(StringAsset.UnavilableTipTable, tool.name, "");
                tool.ShowTipIntruction(unavailableMsg);

                return;
            }

            currentHoldItem = tool;

            InputWrapper.instance.platformInput.SwitchControllerModel(false);
            currentHoldItem.PairToParent(InputWrapper.instance.platformInput.GetParent());

            PaintingManager.EquipTool(StringAsset.GetToolEnumByID(currentHoldItem.name));
        }

        private void OnDirtIsCleared() {
            this.progress += 1;

            PaintingManager.UnEquip();
            InputWrapper.instance.platformInput.SwitchControllerModel(true);

            if (currentHoldItem != null)
                currentHoldItem.Return();

            currentHoldItem = null;

            if (this.progress == maxProgress) {
                DisplayAfterCleanTourGuide();
            }
        }

        private void OnStartBtnClick() {
            GameStartBtn.gameObject.SetActive(false);

            timeline.playableAsset = StartTimeAsset;
            timeline.Play();

            if (skipAnim)
                timeline.time = StartTimeAsset.duration;
        }

        private void UpdateLightDirAccordingToTarget() {
            Vector3 direction = (targetGameObject.transform.position - inputWrapper.cameraObject.transform.position);
            lightingObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        public void ResetLevel() {
            progress = 0;
            PaintingManager.ResetPaint();
        }

    }
}

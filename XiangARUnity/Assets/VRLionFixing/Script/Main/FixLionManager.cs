using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.Input;
using Expect.View;
using Expect.StaticAsset;
using Hsinpa.App;
using Hsinpa.Utility;

namespace Expect.App {
    public class FixLionManager : MonoBehaviour
    {
        [SerializeField]
        FixLionInput FixLionInput;

        [SerializeField]
        PaintingManager PaintingManager;

        [SerializeField]
        private Transform handTransform;

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
        }

        // Update is called once per frame
        void Update()
        {
            FixLionInput.OnUpdate();
        }

        private void DisplayAfterCleanTourGuide() {
            Debug.Log("Clean all done");
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

            currentHoldItem.PairToParent(handTransform);

            PaintingManager.EquipTool(StringAsset.GetToolEnumByID(currentHoldItem.name));
        }

        private void OnDirtIsCleared() {
            this.progress += 1;

            PaintingManager.UnEquip();
            
            if (currentHoldItem != null)
                currentHoldItem.Return();

            currentHoldItem = null;

            if (this.progress == maxProgress) {
                DisplayAfterCleanTourGuide();
            }
        }

        public void ResetLevel() {
            progress = 0;
        }

    }
}

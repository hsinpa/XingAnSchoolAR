using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Expect.Input;
using Expect.View;

namespace Expect.Input {
    public class FixLionManager : MonoBehaviour
    {
        [SerializeField]
        FixLionInput FixLionInput;

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
            FixLionInput.SetUp(_camera);
            FixLionInput.HoldItemEvent += OnTouchToolEvent;
        }

        // Update is called once per frame
        void Update()
        {
            FixLionInput.OnUpdate();
        }

        private void OnTouchToolEvent(ToolItem tool) {
            if (currentHoldItem != null) return;

            currentHoldItem = tool;

            currentHoldItem.PairToParent(handTransform);
        }

        public void ResetLevel() {
            progress = 0;
        }

    }
}

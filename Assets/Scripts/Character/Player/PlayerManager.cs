using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kf {
    public class PlayerManager : CharacterManager {
        PlayerLocomotionManager PlayerLocomotionManager;
        protected override void Awake() {
            base.Awake();
            // DO MORE STUFF, ONLY FOR THIS PLAYER

            PlayerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        protected override void Update() {
            base.Update();

            // IF WE DONT OWN THIS GAME OBJ, WE DONT CONTROL OR EDIT IT
            if(!IsOwner)
                return;

            // HANDLE MOVEMENT
            PlayerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate() {
            if (!IsOwner)
                return;
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraAction();
        }

        public override void OnNetworkSpawn() {
            base.OnNetworkSpawn();

            //  IF THIS IS THE PLAYER OBJ OWN BY THIS CLIENT
            if (IsOwner) {
                PlayerCamera.instance.player = this;
            }
        }
    }
}
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

            // HANDLE MOVEMENT
            PlayerLocomotionManager.HandleAllMovement();
        }
    }
}
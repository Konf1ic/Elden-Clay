using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Kf {
    public class CharacterManager : NetworkBehaviour {

        public CharacterController characterController;

        CharacterNetworkManager characterNetworkManager;
        protected virtual void Awake() {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            characterNetworkManager = GetComponent<CharacterNetworkManager>();
        }

        protected virtual void Update() {
            // IF THIS CHAR IS BEING CONTROLLED FROM OUR SIDE, THEN ASSIGN ITS NETWORK POSITION TO THE POSITION OF OUR TRANSFORM
            if (IsOwner) {
                characterNetworkManager.networkPosition.Value = transform.position;
                characterNetworkManager.networkRotation.Value = transform.rotation;

                // IF THIS CHAR IS BEING CONTROLLED FROM ELSE WHERE, THEN ASSIGN ITS POSITION HERE LOCALLY BY THE POSISTION OF THE NETWORK TRANSFORM
            } else {

                // POSITION
                transform.position = Vector3.SmoothDamp
                    (transform.position,
                    characterNetworkManager.networkPosition.Value,
                    ref characterNetworkManager.networkPositionVerlocity,
                    characterNetworkManager.networkPositionSmoothTime);
                // ROTATION
                transform.rotation = Quaternion.Slerp
                    (transform.rotation,
                    characterNetworkManager.networkRotation.Value,
                    characterNetworkManager.networkRotationSmoothTime);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kf {
    public class PlayerCamera : MonoBehaviour {
        public static PlayerCamera instance;

        public Camera cameraObject;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else {
                Destroy(gameObject);
            }
        }
        private void Start() {
            DontDestroyOnLoad(gameObject);
        }
    }
}
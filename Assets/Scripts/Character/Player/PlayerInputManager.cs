using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kf {
    public class PlayerInputManager : MonoBehaviour {

        public static PlayerInputManager instance;
        // TODO STEPS
        // 2. MOVE CHAR BASE ON THOSE VALUE

        PlayerControls playerControls;

        [Header("PLAYER MOVEMENT INPUT")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        private void Awake() {
            if (instance == null) {
                instance = this;
            } else { 
                Destroy(gameObject);
            }
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);

            // WHEN SCENE CHANGES, RUN THIS LOGIC
            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }

        private void OnSceneChange(Scene oldScene, Scene newScene) {
            // IF WE ARE LOADING TO OUR WORLD SCENE, ENABLE PLAYER CONTROLS
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex()) {
                instance.enabled = true;
                // OTHERWISE WE MUST AT THE MAIN MENU, DISABLE PLAYER CONTROLS
                // THIS FOR THE PLAYER CANT MOVE AROUND WHEN WE ENTER THING LIKE CHARACTER CREATION MENU ETC
            } else { 
                instance.enabled = false;
            }
        }

        private void OnEnable() {
            if (playerControls == null) { 
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy() {
            // IF WE DESTROY THIS OBJ, UNSUB FROM THIS EVENT (TO STOP MEMORY LEAK)
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        // IF WE MINIMIZE OR LOWER THE WINDOW, STOP AJUST INPUTS
        private void OnApplicationFocus(bool focus) {
            if (enabled) {
                if (focus) {
                    playerControls.Enable();
                } else { 
                    playerControls.Disable();
                }
            }
        }

        private void Update() {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
        }

        private void HandlePlayerMovementInput() { 
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            // RETURN ABSOLUTE NUMBER (mean is numbers without negative sign, so always positive)
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            // CLAPM THE VALUES, SO THEY ARE 0, 0.5, 1
            if (moveAmount <= 0.5 && moveAmount > 0) {
                moveAmount = 0.5f;
            } else if(moveAmount > 0.5 && moveAmount <= 1){
                moveAmount = 1;
            }
        }

        private void HandleCameraMovementInput() {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
    }
}
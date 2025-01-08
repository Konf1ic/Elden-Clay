using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kf {
    public class PlayerInputManager : MonoBehaviour {

        public static PlayerInputManager instance;
        // TODO STEPS
        // 2. MOVE CHAR BASE ON THOSE VALUE

        PlayerControls playerControls;

        [SerializeField] Vector2 movementInput;

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

                playerControls.PlayerMovement.Movenent.performed += i => movementInput = i.ReadValue<Vector2>();
            }

            playerControls.Enable();
        }

        private void OnDestroy() {
            // IF WE DESTROY THIS OBJ, UNSUB FROM THIS EVENT (TO STOP MEMORY LEAK)
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
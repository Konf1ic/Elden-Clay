using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace Kf {
    public class PlayerUiManager : MonoBehaviour {

        public static PlayerUiManager instance;

        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;

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

        private void Update() {
            if (startGameAsClient) { 
                startGameAsClient = false;
                // MUST SHUTDOWN AS A HOST BEFORE START AS THE CLIENT
                // AND WE HAS STARTED AS A HOST  DURING THE TITLE SCREEN
                NetworkManager.Singleton.Shutdown();
                // WE RESTART AS A CLIENT
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
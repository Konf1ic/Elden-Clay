using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kf {
    public class WorldSaveGameManager : MonoBehaviour {
        public static WorldSaveGameManager instance;

        [SerializeField] int worldSceneIndex = 1;

        private void Awake() {
            // THERE CAN BE ONLY 1 INSTANCE OF THIS SCRIPT AT 1 TIME, IF ANOTHER EXITS, DESTROY IT 
            if (instance == null) {
                instance = this;
            } else { 
                Destroy(gameObject);
            }
        }

        private void Start() {
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadNewGame() {
            AsyncOperation loadOperator = SceneManager.LoadSceneAsync(worldSceneIndex);

            yield return null;
        }

        public int GetWorldSceneIndex() { 
            return worldSceneIndex;
        }
    }
}
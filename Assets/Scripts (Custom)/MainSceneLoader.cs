using System.Collections;
using System.Collections.Generic;
using TowerDefense.Level;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HoloTD.all { 
    public class MainSceneLoader : MonoBehaviour {

        // Use this for initialization
        public void Awake () {

            loadScene("Level1");

        }

        public void loadScene(string sceneName)
        {
            if (SceneManager.sceneCount > 1)
            {
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            }
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

    }
}

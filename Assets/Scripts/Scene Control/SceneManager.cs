using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SceneControl
{
    public class SceneManager : MonoBehaviour
    {
        private SceneLoader _sceneLoader;

        [SerializeField] private SceneGroupData startScenesToLoadList;

        private List<SceneData> _activeSceneDataList = new List<SceneData>();

        public UnityEvent OnLoadSceneGroupComplete = new UnityEvent();

        public void Construct()
        {
            _sceneLoader = new SceneLoader();

            LoadStartScenes();
        }

        public void LoadStartScenes()
        {
            StartCoroutine(_sceneLoader.LoadSceneGroup(startScenesToLoadList
                , OnLoadSceneGroupComplete));

            OnLoadSceneGroupComplete.AddListener(() =>
            {
                _activeSceneDataList.AddRange(startScenesToLoadList.sceneDataList);
            });
        }
    }
}

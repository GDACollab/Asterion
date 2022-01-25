using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SceneControl
{
    public class SceneLoader
    {
        public IEnumerator LoadSceneGroup(SceneGroupData sceneGroup, UnityEvent callback)
        {
            foreach (SceneData sceneData in sceneGroup.sceneDataList)
            {
                AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager
                .LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
                while (!asyncLoad.isDone)
                    yield return null;
            }

            callback.Invoke();
        }      
    }
}
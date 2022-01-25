using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneControl
{
    [CreateAssetMenu(fileName = "New SceneGroupData", menuName = "Scene Control/SceneGroupData")]
    public class SceneGroupData : ScriptableObject
    {
        public List<SceneData> sceneDataList = new List<SceneData>();
    }
}
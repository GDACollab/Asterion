using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneControl
{
    [CreateAssetMenu(fileName = "New SceneData", menuName = "Scene Control/SceneData")]
    public class SceneData : ScriptableObject
    {
        public string sceneName;
    }
}
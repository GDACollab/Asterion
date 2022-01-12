using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{

    public Transform playerTransform;


    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(playerTransform.localPosition.x, playerTransform.localPosition.y, -10) ;
    }
}

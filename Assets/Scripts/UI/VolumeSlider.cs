using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public float volume;
    public string valueName;

    FMOD.Studio.Bus Master;

    // Start is called before the first frame update
    void Start()
    {
        Master = FMODUnity.RuntimeManager.GetBus("bus:/Master");
        
    }

    // Update is called once per frame
    public void UpdateValue()
    {

        volume = GetComponent<Slider>().value;
        Master.setVolume(volume);
    }
}

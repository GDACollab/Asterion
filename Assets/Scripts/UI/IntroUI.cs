using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{

    public List<Image> blinders;
    public RawImage rt;

    public float fadeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reveal()
    {
        foreach (Image i in blinders)
        {
            LeanTween.value(gameObject, 1f, 0f, fadeTime).setOnUpdate((float val) => {
                i.color = new Color(0,0,0,val);
            });
        }

        LeanTween.value(gameObject, 1f, 0f, fadeTime).setOnUpdate((float val) => {
            rt.color = new Color(0, 0, 0, val);
        });
    }
}

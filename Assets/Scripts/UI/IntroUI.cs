using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroUI : MonoBehaviour
{

    public List<Image> blinders;
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
            LeanTween.value(gameObject, 1f, 0f, 0.8f).setOnUpdate((float val) => {
                i.color = new Color(0,0,0,val);
            });
        }
    }
}

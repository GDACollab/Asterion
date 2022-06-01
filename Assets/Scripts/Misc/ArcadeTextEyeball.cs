using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeTextEyeball : MonoBehaviour
{
    public List<EyeballTest> allEyeballs;
    // Start is called before the first frame update
    void Start()
    {
        ToggleEyeballs(true);
        SetAllText("");
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleEyeballs(false);
            SetAllText("Why do you defile us?");
            
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleEyeballs(true);
            SetAllText("");
        }
        */
    }

    public void ToggleEyeballs(bool setting)
    {
        foreach(EyeballTest e in allEyeballs)
        {
            e.ToggleEye(setting);
        }
    }

    public void ToggleCameras(bool setting)
    {
        foreach (EyeballTest e in allEyeballs)
        {
            e.ToggleCamera(setting);
        }
    }

    public void SetAllText(string s)
    {
        foreach (EyeballTest e in allEyeballs)
        {
            e.SetText(s);
        }
    }

    public void SetAllSize(float s)
    {
        foreach (EyeballTest e in allEyeballs)
        {
            e.SetSize(s);
        }
    }
}

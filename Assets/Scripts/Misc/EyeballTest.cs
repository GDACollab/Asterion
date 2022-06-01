using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EyeballTest : MonoBehaviour
{

    public Renderer planeMat;
    public GameObject player;
    public Vector3 normalizedOffset;
    public TextMeshPro arcadeText;
    public int axis;
    public bool isOn = true;
    [ColorUsageAttribute(true, true)]
    public Color defaultColor;
    [ColorUsageAttribute(true, true)]
    public Color offColor;
    public GameObject fakeCamPlane;
    public bool isCamera = false;


    // Start is called before the first frame update
    void Awake()
    {
        if(arcadeText == null)
        {
            arcadeText = planeMat.transform.GetChild(0).GetComponent<TextMeshPro>();
        }

        if(fakeCamPlane == null)
        {
            fakeCamPlane = planeMat.transform.GetChild(1).gameObject;
        }

        fakeCamPlane.SetActive(false);
    }

    public void ToggleEye(bool setting)
    {
        isOn = setting;

        if (setting)
        {
            planeMat.material.SetColor("_backColor", defaultColor);
        }
        else
        {
            planeMat.material.SetColor("_backColor", offColor);
        }
    }

    public void ToggleCamera(bool setting)
    {
        isCamera = setting;

        if (setting)
        {
            fakeCamPlane.SetActive(true);
        }
        else
        {
            fakeCamPlane.SetActive(false);
        }
    }

    public void SetText(string s)
    {
        arcadeText.text = s;
    }

    public void SetSize(float s)
    {
        arcadeText.fontSize = s;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        normalizedOffset = (player.transform.position - transform.position).normalized;

        
        if(axis == 0)
        {
            planeMat.material.SetVector("_vectorOffset", new Vector3(normalizedOffset.x, 0, 0));
        }
        else if (axis == 1)
        {
            planeMat.material.SetVector("_vectorOffset", new Vector3(-normalizedOffset.z, 0, 0));
        }
        
    }
}

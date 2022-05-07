using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorframe : MonoBehaviour
{
    [SerializeField] private Material _mat;
    [ColorUsage(true, true)]
    [SerializeField] private Color baseColor;

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;
        _mat.SetColor("_EmissionColor", baseColor);
    }

    public void UpdateColor(Color c)
    {
        _mat.SetColor("_EmissionColor", c);
    }
}

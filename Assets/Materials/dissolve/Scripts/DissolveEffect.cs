

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour {

    [SerializeField] private Material material;

    private float dissolveAmount;
    private float dissolveSpeed;
    private bool isDissolving;

    private void Start() {
        if (material == null) {
            material = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        }
    }

    private void Update() {
        if (isDissolving) {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        } else {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - dissolveSpeed * Time.deltaTime);
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }

    public void StartDissolve(float dissolveSpeed, Color dissolveColor) {
        material = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        isDissolving = true;
        material.SetColor("_DissolveColor", dissolveColor);
        this.dissolveSpeed = dissolveSpeed;
    }

    public void StopDissolve(float dissolveSpeed, Color dissolveColor) {
        material = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        isDissolving = false;
        material.SetColor("_DissolveColor", dissolveColor);
        this.dissolveSpeed = dissolveSpeed;
    }

    public void SetEmpty()
    {
        material = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        dissolveAmount = 1;
        material.SetFloat("_DissolveAmount", dissolveAmount);

    }

    public void SetFull()
    {
        material = transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        dissolveAmount = 0;
        material.SetFloat("_DissolveAmount", dissolveAmount);
    }

}

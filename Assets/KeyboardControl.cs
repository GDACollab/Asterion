using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardControl : MonoBehaviour
{
    public float acceleration = 1f;
    public float rotAccel = 5f;
    
    private float totalSpeed;

    private float totalRotation;

    public Rigidbody2D rb;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        totalSpeed = 0;
        Debug.Log(totalSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        totalRotation -= rotAccel * Input.GetAxis("Horizontal");
        totalSpeed += acceleration *  Input.GetAxis("Vertical");
        //movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = totalSpeed;

        if(Input.GetAxis("Vertical") != 0){
            Debug.Log(totalSpeed);
        }
        if(Input.GetAxis("Horizontal") != 0){
            Debug.Log(totalRotation);
        }
        
    }

    void FixedUpdate() {
        //rb.MovePosition(rb.position + movement * acceleration * Time.fixedDeltaTime);
        /*Note to Self; capitalization matters i.e. compile error if spelled DeltaTime or translate/rotate*/
        transform.Translate(0, movement.y * Time.deltaTime, 0);
        transform.Rotate(Vector3.forward * totalRotation * Time.deltaTime);
    }
}

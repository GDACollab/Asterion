using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualCanvasCursor : MonoBehaviour
{
    // Start is called before the first frame update

    public bool cursorEnabled;
    public RectTransform cursor;
    public RectTransform gameCanvas;
    public Vector2 canvasSize;
    public Camera cam;
    public FakeCursor fc;

    void Start()
    {
        canvasSize = new Vector2(gameCanvas.rect.width, gameCanvas.rect.height);
        //m_EventSystem = GetComponent<EventSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (cursorEnabled)
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "RenderTexturePlane")
                {
                    //Debug.Log(hit.textureCoord);


                    Vector2 hitPos = hit.textureCoord * canvasSize;
                    Vector2 center = new Vector2(gameCanvas.rect.width / 2, gameCanvas.rect.height / 2);
                    hitPos -= center;

                    cursor.localPosition = hitPos;


                }



            }


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
               foreach(Button b in fc.collidingObjects)
                {
                    b.onClick.Invoke();
                }
            }
        }


       
    }



}

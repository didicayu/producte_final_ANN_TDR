using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float Sensitivity = 10f;
    public float ZoomSens = 15f;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float HMovement = Input.GetAxisRaw("Horizontal") * Time.deltaTime * Sensitivity;
        float VMovement = Input.GetAxisRaw("Vertical") * Time.deltaTime * Sensitivity;
        float YMovement = -Input.mouseScrollDelta.y * Time.deltaTime * ZoomSens;

        cam.orthographicSize += YMovement;
        
        transform.position += new Vector3(HMovement, 0, VMovement);

        if(cam.orthographicSize <= 1)
        {
            cam.orthographicSize = 1;
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float MaxSpeed = 6f;
    public float TimeZeroToMax = 2.5f;
    float accelerationRate;
    float ForwardVelocity;
    public float RotationSpeed = 5f;
    Rigidbody rb;

    int laps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        accelerationRate = MaxSpeed / TimeZeroToMax;
        ForwardVelocity = 0;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            ForwardVelocity += accelerationRate * Time.deltaTime;
            ForwardVelocity = Mathf.Min(ForwardVelocity, MaxSpeed);
        }
        else if(ForwardVelocity > 0)
        {
            ForwardVelocity -= accelerationRate * Time.deltaTime;
        }
        if(ForwardVelocity <= 0.01f)
        {
            ForwardVelocity = 0f;
        }
    }

    private void FixedUpdate()
    {
       

        if (Input.GetKey(KeyCode.D))
        {
            transform.eulerAngles += new Vector3(0, RotationSpeed * Time.deltaTime, 0);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.eulerAngles -= new Vector3(0, RotationSpeed * Time.deltaTime, 0);
        }
    }

    private void LateUpdate()
    {
        rb.velocity = transform.forward * ForwardVelocity;

    }

    private void OnTriggerEnter(Collider other)
    {
        laps += 1;
        Debug.Log(laps);
    }
}

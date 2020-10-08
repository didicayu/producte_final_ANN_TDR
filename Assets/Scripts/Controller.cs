using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Controller : Agent
{
    public float MaxSpeed = 6f;
    public float TimeZeroToMax = 2.5f;
    float accelerationRate;
    float ForwardVelocity;
    public float RotationSpeed = 5f;
    Rigidbody rb;

    Vector3 StartingPos;
    Quaternion Initialrotation;

    int laps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Initialize()
    {
        
        accelerationRate = MaxSpeed / TimeZeroToMax;
        ForwardVelocity = 0;
        rb = gameObject.GetComponent<Rigidbody>();
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        

        if(vectorAction[0] == 1)
        {
            ForwardVelocity += accelerationRate * Time.deltaTime;
            ForwardVelocity = Mathf.Min(ForwardVelocity, MaxSpeed);
        }
        if(vectorAction[1] == 0)
        {
            //rotate to the right
            transform.eulerAngles += new Vector3(0, RotationSpeed * Time.deltaTime, 0);

        }
        if(vectorAction[1] == 1)
        {
            //rotate to the left
            transform.eulerAngles -= new Vector3(0, RotationSpeed * Time.deltaTime, 0);

        }
        if(vectorAction[1] == 2)
        {
            rb.angularVelocity = Vector3.zero;
        }

    }

    public override void OnEpisodeBegin()
    {
        //Reset environment
        Reset();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0;
        actionsOut[1] = 0;
        //Player Input
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        else if (ForwardVelocity > 0)
        {
            ForwardVelocity -= accelerationRate * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[1] = 0;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            actionsOut[1] = 1;
        }
        else
        {
            actionsOut[1] = 2;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //1 float 
        sensor.AddObservation(gameObject.transform.rotation.y);
        //3 floats
        sensor.AddObservation(rb.velocity);
        
    }


    private void Awake()
    {
        StartingPos = gameObject.transform.position;
        Initialrotation = transform.rotation;
    }


    public void Reset()
    {
        transform.position = StartingPos;
        transform.rotation = Initialrotation;
        ForwardVelocity = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Untagged"))
        {
            AddReward(-1.0f);
            //Debug.Log(GetCumulativeReward());
            EndEpisode();
        }

    }
    private Transform arr;

    int AreWeLookingTheRightWay()
    {

        int reward = 0;

        if(Physics.Raycast(transform.position, Vector3.down, out var hit, 2f))
        {

            var NewHit = hit.transform.Find("ExitVector");

            if(arr != null && NewHit != arr)
            {

                float angle = Quaternion.Angle(transform.rotation, NewHit.rotation);
                

                //Debug.Log("ang: " + angle);
                reward = (angle >= 95f) ? 1 : -1;
            }

            arr = NewHit;

        }
        

        return reward;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        float directionInt = AreWeLookingTheRightWay();
        if (ForwardVelocity <= 0.01f)
        {
            ForwardVelocity = 0f;
        }

        if (directionInt < 0)
        {
            AddReward(-1.0f);
            //Debug.Log(GetCumulativeReward());
            EndEpisode();
        }
        else if (directionInt > 0)
        {
            AddReward(1f);
            AddReward((rb.velocity.magnitude / 500f));
            //Debug.Log((rb.velocity.magnitude / 500f));
        }
        //Debug.Log(GetCumulativeReward());
        
    }

    private void LateUpdate()
    {
        rb.velocity = transform.forward * ForwardVelocity;

    }

    private void OnTriggerEnter(Collider other)
    {
        
        laps += 1;
        Debug.Log("Num of laps: " + laps + "!");
        if(laps > 1)
        {
            AddReward(laps * laps);
        }
        else
        {
            AddReward(2f);
        }
    }
}

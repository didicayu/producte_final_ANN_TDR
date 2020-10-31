using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Controller2 : Agent
{
    public float MaxSpeed = 6f;
    public float TimeZeroToMax = 2.5f;
    float accelerationRate;
    float DecelerationRate;
    float ForwardVelocity;
    public float RotationSpeed = 5f;
    Rigidbody rb;
    Vector3 StartingPos;
    Quaternion Initialrotation;

    public int score;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Initialize()
    {
        StartingPos = transform.position;
        DecelerationRate = (3 * MaxSpeed) / TimeZeroToMax;
        accelerationRate = MaxSpeed / TimeZeroToMax;
        ForwardVelocity = 0;
        rb = gameObject.GetComponent<Rigidbody>();
        AreWeLookingTheRightWay();
    }


    public override void OnActionReceived(float[] vectorAction)
    {

        if (vectorAction[0] == 0)
        {
            //ForwardVelocity -= DecelerationRate * Time.fixedDeltaTime;
            //ForwardVelocity = Mathf.Max(0, ForwardVelocity);
            ForwardVelocity = 0;
        }
        if (vectorAction[0] == 1)
        {
            ForwardVelocity += accelerationRate * Time.deltaTime;
            ForwardVelocity = Mathf.Min(ForwardVelocity, MaxSpeed);
        }
        else if (ForwardVelocity > 0)
        {
            //ForwardVelocity -= accelerationRate * Time.fixedDeltaTime;
        }
        if (vectorAction[1] == 0)
        {
            //rotate to the right
            float rotation = 1 * RotationSpeed * 90f;
            transform.Rotate(0f, rotation * Time.fixedDeltaTime, 0f);
            //transform.eulerAngles += new Vector3(0, RotationSpeed * Time.deltaTime, 0);

        }
        if (vectorAction[1] == 1)
        {
            //rotate to the left
            float rotation = -1 * RotationSpeed * 90f;
            transform.Rotate(0f, rotation * Time.fixedDeltaTime, 0f);
            //transform.eulerAngles -= new Vector3(0, RotationSpeed * Time.deltaTime, 0);

        }
        if (vectorAction[1] == 2)
        {
            rb.angularVelocity = Vector3.zero;
        }



        int reward = AreWeLookingTheRightWay();

        var moveVec = transform.position - lastPos;

        float angle = Vector3.Angle(moveVec, _track.up);

        //float bonus = (1f - angle / 90f) * Time.fixedDeltaTime * vectorAction[0];

        AddReward(reward);

        score += reward;
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
            //ForwardVelocity -= accelerationRate * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 0;
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
        //Debug.Log(actionsOut[0] + " " + actionsOut[1]);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        float angle = Vector3.SignedAngle(_track.up, transform.forward, Vector3.up);


        //3 floats
        sensor.AddObservation(rb.velocity);


        //1float
        sensor.AddObservation(angle / 180f);

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
            SetReward(-1.0f);
            //Debug.Log(GetCumulativeReward());

            EndEpisode();

        }

    }
    private Transform _track;
    float angle;
    Vector3 lastPos;
    private int AreWeLookingTheRightWay()
    {
        int reward = 0;


        // Find what tile I'm on
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 2f))
        {
            var newHit = hit.transform.Find("ExitVector");
            // Check if the tile has changed
            if (_track != null && newHit != _track)
            {
                lastPos = transform.position;
                angle = Vector3.Angle(_track.up, newHit.position - _track.position);

                reward = (angle < 90f) ? 1 : -1;
            }

            _track = newHit;
        }

        return reward;
    }

    // Update is called once per frame



    private void LateUpdate()
    {
        rb.velocity = transform.forward * ForwardVelocity;
        if (ForwardVelocity < 0.001)
        {
            ForwardVelocity = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {


    }
}

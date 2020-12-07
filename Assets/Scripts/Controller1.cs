using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Controller1 : Agent
{
    Rigidbody rb;
    public float speed = 10f;
    public float torque = 10f;

    public int score = 0;
    //public bool resetOnCollision = true;

    private Transform _track;
    Vector3 StartingPos;
    Quaternion Initialrotation;
    // Start is called before the first frame update
    int laps;
    void Start()
    {
        
    }
    private void MoveCar(float horizontal, float vertical, float dt)
    {
        float distance = speed * vertical;
        transform.Translate(distance * dt * Vector3.forward);

        float rotation = horizontal * torque * 90f;
        transform.Rotate(0f, rotation * dt, 0f);
    }
    public override void Initialize()
    {
        GetTrackIncrement();
        rb = gameObject.GetComponent<Rigidbody>();
        
    }


    public override void OnActionReceived(float[] vectorAction)
    {
        float horizontal = vectorAction[0];
        float vertical = vectorAction[1];

        var lastPos = transform.position;
        MoveCar(horizontal, vertical, Time.fixedDeltaTime);

        int reward = GetTrackIncrement();

        var moveVec = transform.position - lastPos;
        float angle = Vector3.Angle(moveVec, _track.up);
        
        float bonus = (1f - angle / 90f) * Mathf.Clamp01(vertical) * Time.fixedDeltaTime;
        //Debug.Log(reward + bonus);
        AddReward(bonus + reward);
        score += reward;

    }

    public override void OnEpisodeBegin()
    {
        //Reset environment
        Reset();
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Horizontal");
        actionsOut[1] = Input.GetAxis("Vertical");
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

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Untagged") || coll.gameObject.CompareTag("Wall"))
        {
            AddReward(-1f);
            //Debug.Log(GetCumulativeReward());
            
            EndEpisode();
            Debug.Log(GetCumulativeReward());

        }

    }
    private int GetTrackIncrement()
    {
        int reward = 0;
       

        // Find what tile I'm on
        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 2f))
        {
            var newHit = hit.transform.Find("ExitVector");
            // Check if the tile has changed
            if (_track != null && newHit != _track)
            {
                float angle = Vector3.Angle(_track.up, newHit.position - _track.position);
                reward = (angle < 90f) ? 1 : -1;
            }

            _track = newHit;
        }

        return reward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
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
        */
    }
}

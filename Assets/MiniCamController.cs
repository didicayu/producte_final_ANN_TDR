using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamController : MonoBehaviour
{
    public GameObject ParentObject;
    Vector3 sum;
    float FurtherAway;
    public float offset = 8f;
    // Start is called before the first frame update
    void Start()
    {
        FurtherAway = 0;
        sum = Vector3.zero;
        StartCoroutine(WaitFrames());
    }
    IEnumerator WaitFrames()
    {
        yield return 1;

        DoPositionCalculationStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DoPositionCalculationStuff()
    {
        for (int i = 0; i < ParentObject.transform.childCount; i++)
        {
            sum += ParentObject.transform.GetChild(i).position;
            float result = Mathf.Max(Mathf.Abs(ParentObject.transform.GetChild(i).position.z - transform.position.z), FurtherAway);
            FurtherAway = result;
        }
        sum = (sum / ParentObject.transform.childCount);

        transform.position = new Vector3(sum.x, transform.position.y, sum.z);
        gameObject.GetComponent<Camera>().orthographicSize = FurtherAway - offset;
    }
}

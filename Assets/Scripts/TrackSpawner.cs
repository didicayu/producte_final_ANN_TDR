using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner : MonoBehaviour
{
    [Header("Nom del circuit")]
    public string TrackName = "track";

    public GameObject ParentObject;

    public GameObject[] Objectes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        ParentObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SpawnTrack();
        }
    }

    void SpawnTrack()
    {
        ParentObject.transform.localScale = new Vector3(1,1,1);

        for (int j = 0; j < ParentObject.transform.childCount; j++)
        {
            Transform trans = ParentObject.transform.GetChild(j);
            Destroy(trans.gameObject);
        }

        TrackData data = SaveSystem.LoadTrackWithName(TrackName);

        for (int i = 0; i < data.TypesOfRoad.Length; i++)
        {
            GameObject go = Instantiate(Objectes[(int.Parse(data.TypesOfRoad[i]))], ParentObject.transform);
            go.transform.position = new Vector3(data.positionsX[i], data.positionsY[i], data.positionsZ[i]);
            go.transform.eulerAngles = new Vector3(0, data.rotation[i], 0);
            go.name = data.TypesOfRoad[i];
        }

        ParentObject.transform.localScale *= 5;
    }
}

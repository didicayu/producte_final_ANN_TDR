using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackSpawner1 : MonoBehaviour
{
    [Header("Nom del circuit")]
    public string TrackName = "track";

    public GameObject ParentObject;
    [Header("Ficar els prefabs de les carreteres i l'agent")]
    public GameObject[] Objectes;
    public GameObject AgentCar;
    public float HeightOffset = 1.75f;

    // Start is called before the first frame update
    void Start()
    {
        //AgentCar.SetActive(true);
        StartCoroutine(WaitXFrames(1));
    }
    IEnumerator WaitXFrames(int NumOfFrames)
    {
        
        yield return NumOfFrames;
        AgentCar.SetActive(true);

    }

    private void Awake()
    {
        TrackName = PlayerPrefs.GetString("TrackName");
        ParentObject = gameObject;
        SpawnTrack();
        Transform three = gameObject.transform.Find("3");

        AgentCar.transform.position = new Vector3(three.position.x, three.position.y + HeightOffset, three.position.z);
        AgentCar.transform.rotation = three.transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        
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

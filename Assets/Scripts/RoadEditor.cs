using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoadEditor : MonoBehaviour
{
    public GameObject[] Objectes;
    public GameObject[] PlaceHolders;
    public GameObject ParentObject;
    public GameObject[] roads;
    Camera cam;

    [HideInInspector]
    public int ObjNum = 0;

    public string TrackName;

    Vector3 pos;
    Vector3 OldPos;
    Vector3[] StartingPos;

    GameObject[] ro;

    int rotationRoad;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;


        StartingPos = new Vector3[PlaceHolders.Length];

        for (int i = 0; i < PlaceHolders.Length; i++)
        {
            StartingPos[i] = PlaceHolders[i].transform.position;
        }
        //agafa les posicions inicials de les carreteres fantasma
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GhostRoad();
        //calcular la posició a la que es ficaran les carreteres
        


        //selecciona el nombre del objecte si existeix
        if (Input.anyKeyDown)
        {
            for (int i = 1; i < 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()) && i <= Objectes.Length)
                {
                    ObjNum = i - 1;
                }
            }
        }

        //spawneje una carretera en cas de que no hi hagui cap pel mig
        
        //elimina la carretera
        if (Input.GetMouseButton(1))
        {
            DestroyRoad();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotationRoad -= 90;
            if(rotationRoad == -360)
            {
                rotationRoad = 0;
            }
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            rotationRoad += 90;
            if (rotationRoad == 360)
            {
                rotationRoad = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            SaveRoads();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            LoadRoad();
        }

    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && RoadPresent() == false && !EventSystem.current.IsPointerOverGameObject())
        {
            if (RoadPresent() == false)
            {
                SpawnObject(ObjNum);        
            }
        }
    }

    void SpawnObject(int ObjNum)
    {
        ro = GameObject.FindGameObjectsWithTag("Road");
        GameObject go = Instantiate(Objectes[ObjNum], pos, PlaceHolders[ObjNum].transform.rotation, ParentObject.transform);

        //preveu que apareguin dues carreteres al mateix lloc
        for (int i = 0; i < ro.Length; i++)
        {
            if(Vector3.Distance(ro[i].transform.position, pos) == 0)
            {
                Destroy(go);
            }
        }
        go.name = ObjNum.ToString();
    }

    public void DeleteAllRoads()
    {
        ro = GameObject.FindGameObjectsWithTag("Road");
        for (int i = 0; i < ro.Length; i++)
        {
            Destroy(ro[i]);
        }
    }

    //mire si hi ha una carretera i la destrueix
    void DestroyRoad()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Road")
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }

    //mire si hi ha una carretera present, en cas afirmatiu no deixarem crear una altra per sobre
    bool RoadPresent()
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Road")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    void GhostRoad()
    {
        
        pos = cam.ScreenToWorldPoint(Input.mousePosition);
       
        pos = new Vector3(Mathf.RoundToInt(pos.x), 0, Mathf.RoundToInt(pos.z));
        
        PlaceHolders[ObjNum].transform.eulerAngles = new Vector3(0, rotationRoad, 0);
        PlaceHolders[ObjNum].transform.position = pos;

        for (int i = 0; i < PlaceHolders.Length; i++)
        {
            if(PlaceHolders[i] != PlaceHolders[ObjNum])
            {
                PlaceHolders[i].transform.position = StartingPos[i];
            }
        }
        

    }


    public void SaveRoads()
    {
        roads = new GameObject[ParentObject.transform.childCount];

        for (int i = 0; i < roads.Length; i++)
        {
            roads[i] = ParentObject.transform.GetChild(i).gameObject;
        }


        SaveSystem.SaveTrack(this);
        Debug.Log(Application.persistentDataPath + "/" + TrackName + ".didac");
    }

    public void LoadRoad()
    {

        for (int j = 0; j < ParentObject.transform.childCount; j++)
        {
            Transform trans = ParentObject.transform.GetChild(j);
            Destroy(trans.gameObject);
        }

        TrackData data = SaveSystem.LoadTrack(this);

        for (int i = 0; i < data.TypesOfRoad.Length; i++)
        {
            GameObject go = Instantiate(Objectes[(int.Parse(data.TypesOfRoad[i]))], ParentObject.transform);
            go.transform.position = new Vector3(data.positionsX[i], data.positionsY[i], data.positionsZ[i]);
            go.transform.eulerAngles = new Vector3(0, data.rotation[i], 0);
            go.name = data.TypesOfRoad[i];
        }
    }

}

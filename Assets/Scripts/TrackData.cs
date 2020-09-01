using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrackData
{
    public string[] TypesOfRoad;

    public float[] positionsX;
    public float[] positionsY;
    public float[] positionsZ;

    public float[] rotation;

    public TrackData(RoadEditor roadED)
    {
        TypesOfRoad = new string[roadED.roads.Length];
        rotation = new float[roadED.roads.Length];

        positionsX = new float[roadED.roads.Length];
        positionsY = new float[roadED.roads.Length];
        positionsZ = new float[roadED.roads.Length];

        for (int i = 0; i < TypesOfRoad.Length; i++)
        {
            TypesOfRoad[i] = roadED.roads[i].name;
            rotation[i] = roadED.roads[i].transform.eulerAngles.y;

            positionsX[i] = roadED.roads[i].transform.position.x;
            positionsY[i] = roadED.roads[i].transform.position.y;
            positionsZ[i] = roadED.roads[i].transform.position.z;
        }
    }
}

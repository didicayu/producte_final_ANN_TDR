using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{

    public static void SaveTrack(RoadEditor RoadED)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/"+ RoadED.TrackName +".didac";
        FileStream stream = new FileStream(path, FileMode.Create);

        TrackData data = new TrackData(RoadED);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static TrackData LoadTrack(RoadEditor RoadED)
    {
        string path = Application.persistentDataPath + "/" + RoadED.TrackName + ".didac";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            TrackData data = formatter.Deserialize(stream) as TrackData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("no es pot trobar l'arxiu a: " + path);
            return null;
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public RoadEditor re;

    public void TestTrack()
    {
        Transform[] children = re.ParentObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if(children[i].name == "3")
            {
                PlayerPrefs.SetString("TrackName", re.TrackName);
                PlayerPrefs.Save();

                StartCoroutine(LoadAsynchronously("TrackPlayer"));
            }
            else
            {
                Debug.LogError("Cal que posis un començament");
            }
        }

    }

    IEnumerator LoadAsynchronously(string SceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);

        while(op.isDone == false)
        {
            Debug.Log(op.progress);

            yield return null;
        }
    }
}

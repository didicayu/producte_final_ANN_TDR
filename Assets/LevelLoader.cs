using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public RoadEditor re;

    public GameObject LoadingScreen;
    public Slider slider;

    public void TestTrack()
    {
        Transform[] children = re.ParentObject.GetComponentsInChildren<Transform>();
        bool CanLoadLevel = false;
        for (int i = 0; i < children.Length; i++)
        {
            if(children[i].name == "3")
            {
                PlayerPrefs.SetString("TrackName", re.TrackName);
                PlayerPrefs.Save();
                CanLoadLevel = true;
                StartCoroutine(LoadAsynchronously("TrackPlayer"));
            }
        }
        if (CanLoadLevel == false)
        {
            Debug.LogError("Cal que posis un començament");
        }

    }

    IEnumerator LoadAsynchronously(string SceneName)
    {
        LoadingScreen.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(SceneName);

        while(op.isDone == false)
        {
            float progress = Mathf.Clamp01(op.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }
}

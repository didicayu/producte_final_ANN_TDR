using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLevelLoader : MonoBehaviour
{
    public Animator transition;
    float AmountOfSec = 1f;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCarEditor()
    {
        StartCoroutine(LoadLevel("TrackMaker"));
    }

    IEnumerator LoadLevel(string LevelName)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(AmountOfSec);

        SceneManager.LoadScene(LevelName);
    }
}

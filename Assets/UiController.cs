using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    string path;

    public Image[] ImgBoxes;
    public Sprite Selected;
    public Sprite Deselected;
    RoadEditor re;
    int ObjNum;

    public TMP_Dropdown m_dropDown;
    public TMP_InputField TmInput;

    List<string> m_DropDownOptions;
    // Start is called before the first frame update
    private void Awake()
    {
        path = (Application.persistentDataPath + "/");
        re = gameObject.GetComponent<RoadEditor>();
        GetSavedFiles();
    }
    void Start()
    {
        ImgBoxes[0].sprite = Selected;

        
    }


    public void GetSavedFiles()
    {
        m_DropDownOptions = new List<string>() {"Escolleix Circuit"};
        m_dropDown.ClearOptions();

        foreach (string file in Directory.GetFiles(path))
        {
            m_DropDownOptions.Add(Path.GetFileNameWithoutExtension(file));
        }
        m_dropDown.AddOptions(m_DropDownOptions);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_dropDown.gameObject.transform.childCount != 3)
        {
            Toggle[] dropChild = m_dropDown.GetComponentsInChildren<Toggle>();
            dropChild[0].interactable = false;
        }
        if (Input.anyKeyDown)
        {
            for (int i = 1; i < ImgBoxes.Length + 1; i++)
            {
                if (Input.GetKeyDown(i.ToString()) && i <= ImgBoxes.Length)
                {
                    ObjNum = i - 1;
                    ImgBoxes[ObjNum].sprite = Selected;
                    for (int k = 0; k < ImgBoxes.Length; k++)
                    {
                        if(k != ObjNum)
                        {
                            ImgBoxes[k].sprite = Deselected;
                        }
                    }
                }
            }
        }
    }
    public void SelectRoad(int roadNum)
    {
        for (int k = 0; k < ImgBoxes.Length; k++)
        {
            if (k != roadNum)
            {
                ImgBoxes[k].sprite = Deselected;
            }
        }
        ImgBoxes[roadNum].sprite = Selected;
        re.ObjNum = roadNum;
    }

    public void Save()
    {
        if(TmInput.text != "")
        {
            re.TrackName = TmInput.text;
            re.SaveRoads();
            GetSavedFiles();
        }
        else
        {
            Debug.LogWarning("Per guardar cal asignar un nom al circuit");
        }
    }
    public void Delete()
    {
        re.DeleteAllRoads();
    }
    public void Load(string TrackSaveName)
    {
        re.TrackName = TrackSaveName;
        re.LoadRoad();
    }
    public void HandleDropDown(int val)
    {
        if(val != 0)
        {
            Load(m_DropDownOptions[val]);
        }
        
    }


}

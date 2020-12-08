using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    public float floatingSpeed;
    public bool Dox;
    public bool Doy;
    public bool Doz;
    int ics;
    int igrega;
    int zeta;

    public bool sentitDeGirPos;
    // Start is called before the first frame update
    void Start()
    {
        

        if (Dox == true)
        {
            ics = 1;
        }
        else
        {
            ics = 0;
        }

        if (Doy == true)
        {
            igrega = 1;
        }
        else
        {
            igrega = 0;
        }

        if (Doz == true)
        {
            zeta = 1;
        }
        else
        {
            zeta = 0;
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        Vector3 rot = new Vector3(ics * floatingSpeed * Time.deltaTime, igrega * floatingSpeed * Time.deltaTime, zeta * floatingSpeed * Time.deltaTime);
        if(sentitDeGirPos == true)
        {
            transform.Rotate(rot * 1);
        }
        else
        {
            transform.Rotate(rot * -1);
        }
        
    }
}

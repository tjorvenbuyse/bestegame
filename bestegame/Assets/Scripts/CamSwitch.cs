using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    // Start is called before the first frame update
    void Start()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.C))
        {
            cam2.SetActive(true);
            cam1.SetActive(false);
        }
        if (Input.GetKey(KeyCode.V))
        {
            cam1.SetActive(true);
            cam2.SetActive(false);
        }
    }
}

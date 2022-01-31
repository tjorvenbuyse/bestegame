using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPSTarget : MonoBehaviour
{
    public float damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        damage = 0;
    }


    public void dpsScore(float damage2)
    {
        damage += damage2;
    }
}

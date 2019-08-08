using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatTailWind : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //GetComponent<Rigidbody>().AddForce(transform.forward*100, ForceMode.Force);
    }

    public void SendForce(Vector3 force)
    {
        //print(force*1000);
        GetComponent<Rigidbody>().AddForce(new Vector3(-force.x, -force.z, 0)*5, ForceMode.Impulse);
        //GetComponent<Rigidbody>().AddForce(force*1000);
    }
}

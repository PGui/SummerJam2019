using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceZeroWorldRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(90.0f,-transform.parent.transform.parent.rotation.eulerAngles.y,0);
    }
}

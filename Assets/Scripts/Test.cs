using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(cube.transform.position.x, 5 + i * 5 , cube.transform.position.z);
            cube.AddComponent<Rigidbody>();
			//cou cou c'est Ludo
        }
    }
    //Bastien Comment submit test
    // Update is called once per frame
    void Update()
    {
        
    }
}

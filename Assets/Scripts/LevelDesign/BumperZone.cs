using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperZone : MonoBehaviour
{
    
    public float jumpMultiplier = 1.5f;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
    
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.jumpMultiplier*= jumpMultiplier;
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.jumpMultiplier/= jumpMultiplier;
        }
    }
}

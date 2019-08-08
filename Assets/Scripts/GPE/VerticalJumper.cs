using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalJumper : MonoBehaviour
{
    public bool forceDirectionToArenaCenter = false;
    public float verticalMultiplier = 5.0f;
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
        if (other.gameObject.tag != "Player")
            return;

        GameObject otherEntity = other.transform.gameObject;
        otherEntity.GetComponent<PlayerController>().TriggerOutOfArenaBump(verticalMultiplier, forceDirectionToArenaCenter);
    }
}

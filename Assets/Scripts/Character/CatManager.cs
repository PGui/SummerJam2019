using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatManager : MonoBehaviour
{
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
        if (other.gameObject.tag != "TouchCollider")
            return;

        GameObject otherEntity = other.transform.parent.gameObject;
        CatState.eCatState otherState = otherEntity.GetComponent<CatState>().currentState;
        
        if (otherState == CatState.eCatState.CHASED)
        {
            otherEntity.GetComponent<CatState>().currentState = CatState.eCatState.CHASER;
        }
    }
}

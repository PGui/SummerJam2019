using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatState : MonoBehaviour
{

    /*[ReadOnly]*/ public eCatState currentState = eCatState.NONE;

    void Start()
    {
        this.GetComponentInChildren<CatCollider>().DelegateChaser += SetAsChaser;
    }
    
    public void SetAsChaser(GameObject touchedChaserCat)
    {
        currentState = eCatState.CHASER;
    }
}

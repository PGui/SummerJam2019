using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void DelegateToChaser(GameObject touchedChasedCat);
public class CatCollider : MonoBehaviour
{
	public DelegateToChaser DelegateChaser;
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
        Scene scene = SceneManager.GetActiveScene();
        if (other.gameObject.tag != "TouchCollider" || scene.name == "CharacterSelection")
            return;

        GameObject otherEntity = other.transform.parent.gameObject;
        eCatState otherState = otherEntity.GetComponent<CatState>().currentState;        
        if (otherState == eCatState.CHASER && otherEntity.GetComponent<PlayerController>().canCapture)
        {
            this.gameObject.GetComponentInParent<CatState>().currentState = eCatState.CHASER;
            DelegateChaser(otherEntity);
        }
    }
}

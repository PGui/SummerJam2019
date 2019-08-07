using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
// This fake class is linked in Player Manager to do some stuff on characters during selection (change UI image, change player color, select ability...anything we might need)
// It must be placed on player prefab to handle the character selection
// Functions Select Next/Previous character will handle the DPad Horizontal inputs of players during character selection
//
//
public class CharacterSelector : MonoBehaviour
{
    [ReadOnly] int currentSelectedCharacterID = 0;
    public GameObject chaserMesh; 
    public GameObject chasedMesh; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CatState catState = this.GetComponent<CatState>();
        if(catState != null)
        {
            if(catState.currentState == eCatState.CHASER)
            {
                if(!chaserMesh.activeInHierarchy)
                {
                    chaserMesh.SetActive(true);
                    chasedMesh.SetActive(false);
                }
            }
            else if(catState.currentState == eCatState.CHASED)
            {
                if(!chasedMesh.activeInHierarchy)
                {
                    chaserMesh.SetActive(false);
                    chasedMesh.SetActive(true);
                }
            }
        }
    }
    
    public void HideAllMeshes()
    {
        chaserMesh.SetActive(false);
        chasedMesh.SetActive(false);
    }
    public void OnDPadLeftPressed()
    {
        currentSelectedCharacterID--;
        //print(string.Format("Player {0} changed character (LEFT) // this function does nothing for now", this.gameObject.GetComponent<PlayerController>().playerControllerID));
        if(this.gameObject.GetComponent<CatState>().currentState == eCatState.CHASER)
        {
            this.gameObject.GetComponent<CatState>().currentState = eCatState.CHASED;
        }
        else if(this.gameObject.GetComponent<CatState>().currentState == eCatState.CHASED)
        {
            this.gameObject.GetComponent<CatState>().currentState = eCatState.CHASER;
        }
    }
    public void OnDPadRightPressed()
    {
        currentSelectedCharacterID++;
        //print(string.Format("Player {0} changed character (RIGHT) // this function does nothing for now", this.gameObject.GetComponent<PlayerController>().playerControllerID));        
        if(this.gameObject.GetComponent<CatState>().currentState == eCatState.CHASER)
        {
            this.gameObject.GetComponent<CatState>().currentState = eCatState.CHASED;
        }
        else if(this.gameObject.GetComponent<CatState>().currentState == eCatState.CHASED)
        {
            this.gameObject.GetComponent<CatState>().currentState = eCatState.CHASER;
        }
    }
}

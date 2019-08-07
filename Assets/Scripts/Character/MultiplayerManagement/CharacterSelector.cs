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
    [System.Serializable]
    public class ColoredMeshPair
    {
        public GameObject chaserMesh; 
        public GameObject chasedMesh;
        public Color pairColor;
    }
    [ReadOnly] int currentSelectedCharacterID = 0;
    public GameObject chaserMesh; 
    public GameObject chasedMesh; 
    public ColoredMeshPair[] coloredMeshes; 
    public int currentColorVariantID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CatState catState = this.GetComponent<CatState>();
        if(catState != null && chaserMesh != null && chasedMesh != null)
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
        if(chaserMesh != null && chasedMesh != null)
        {
            chaserMesh.SetActive(false);
            chasedMesh.SetActive(false);
        }
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
    public void OnDPadDownPressed()
    {
        HideAllMeshes();
        currentColorVariantID--;
        if(currentColorVariantID < 0)
        {
            currentColorVariantID = coloredMeshes.Length-1;
        }
        if(currentColorVariantID < coloredMeshes.Length)
        {
            chaserMesh = coloredMeshes[currentColorVariantID].chaserMesh;
            chasedMesh = coloredMeshes[currentColorVariantID].chasedMesh;
        }
    }
    public void OnDPadUpPressed()
    {
        HideAllMeshes();
        currentColorVariantID++;
        if(currentColorVariantID == coloredMeshes.Length)
        {
            currentColorVariantID = 0;
        }
        if(currentColorVariantID < coloredMeshes.Length)
        {
            chaserMesh = coloredMeshes[currentColorVariantID].chaserMesh;
            chasedMesh = coloredMeshes[currentColorVariantID].chasedMesh;
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
    public Color GetCatColor()
    {
        return coloredMeshes[currentColorVariantID].pairColor;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//
// This fake class is linked in Player Manager to do some stuff on characters during selection (change UI image, change player color, select ability...anything we might need)
// It must be placed on player prefab to handle the character selection
// Functions Select Next/Previous character will handle the DPad Horizontal inputs of players during character selection
//
//
public class FakeCharacterSelector : MonoBehaviour
{
    [ReadOnly] int currentSelectedCharacterID = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDPadLeftPressed()
    {
        currentSelectedCharacterID--;
        print(string.Format("Player {0} changed character (LEFT) // this function does nothing for now", this.gameObject.GetComponent<FakePlayerController>().playerControllerID));
    }
    public void OnDPadRightPressed()
    {
        currentSelectedCharacterID++;
        print(string.Format("Player {0} changed character (RIGHT) // this function does nothing for now", this.gameObject.GetComponent<FakePlayerController>().playerControllerID));        
    }
}

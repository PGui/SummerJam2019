using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;


//
//
// This fake class is linked in Player Manager to pass the controller ID and enable the controller from Inputs to Player's entities
// Variables must be copied in real PlayerController.cs
// Update must check gameplayControlsEnabled (Menu / Gameplay separation)
//
//
public class FakePlayerController : MonoBehaviour
{
    [ReadOnly] public int playerControllerID = 0;
    [ReadOnly] public bool gameplayControlsEnabled = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameplayControlsEnabled)
        {
            if(Input.GetButtonDown(PlayerManager.K_DASH+playerControllerID.ToString()))
            {
                print("DASH");
            }
            if(Input.GetButtonDown(PlayerManager.K_JUMP+playerControllerID.ToString()))
            {
                print("JUMP");
            }
            //...
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LocalCoop;

[System.Serializable]
public class PlayerData
{    
    public int controllerID = 0;
    public int score = 0;
    public bool isActive = false; //Used to sapwn/unspawn characters
    public bool isReady = false; //Used to set player as "ready" (when all are ready, Load Level scene)
    private bool isDPadPressed = false; //Handle the release of DPad
    public GameObject playerGameObject;
    public void Spawn(GameObject PlayerPrefab)
    {
        //Spawn and store player prefab in GameObject
        //Set the Player ID in PlayerController for input strings matching
        GameObject player = GameObject.Instantiate(PlayerPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
        player.transform.parent = GameObject.Find("PlayerManager").transform;
        player.GetComponent<PlayerController>().playerControllerID = controllerID.ToString();
        playerGameObject = player;
        SceneManager.sceneLoaded += OnSceneLoaded;
        player.GetComponentInChildren<CatCollider>().DelegateChaser += IncreaseScore;

        if(controllerID == 1)
        {
            playerGameObject.GetComponent<CatState>().currentState = eCatState.CHASER;
        }
    }
    void IncreaseScore(GameObject touchedChasedCat)
    {
        GameObject playerManager = GameObject.Find("PlayerManager");
        if(playerManager != null)
        {
            playerManager.GetComponent<PlayerManager>().IncreaseScore(int.Parse(touchedChasedCat.GetComponent<PlayerController>().playerControllerID));
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerManager.IsMenuScene(scene))
        {
            TeleportAtSpawnPosition();
        }
    }
    public void Activate()
    {
        isActive = true;
    }
    public void Ready()
    {
        isReady = true;
    }
    public void Deactivate()
    {
        isActive = false;
        isReady = false;
    }
    public void TeleportAtSpawnPosition()
    {
        GameObject spawner = GameObject.Find("Player"+controllerID.ToString()+"Spw");
        if(spawner != null && playerGameObject != null)
        {
            playerGameObject.transform.position = spawner.transform.position;
        }
    }
    public void ToggleGameplayControls(bool isEnabled)
    {
        playerGameObject.GetComponent<PlayerController>().gameplayControlsEnabled = isEnabled;
    }
    public void ReleaseDPad()
    {
        isDPadPressed = false;
    }
    public void SelectNextCharacter()
    {
        if(!isDPadPressed)
        {
            isDPadPressed = true;
            playerGameObject.GetComponent<FakeCharacterSelector>().OnDPadRightPressed();
        }
    }
    public void SelectPreviousCharacter()
    {
        if(!isDPadPressed)
        {
            isDPadPressed = true;
            playerGameObject.GetComponent<FakeCharacterSelector>().OnDPadLeftPressed();
        }
    }
    public void Reset()
    {  
        isActive = false; 
        isReady = false;
        ToggleGameplayControls(false);
        if(controllerID != 1)
        {
            playerGameObject.GetComponent<CatState>().currentState = eCatState.CHASED;
        }
    }
    public void OnDisable()
    {  
        controllerID = 0;
        score = 0;
        isActive = false;
        isReady = false;
        isDPadPressed = false; //Handle the release of DPad
        GameObject.DestroyImmediate(playerGameObject);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
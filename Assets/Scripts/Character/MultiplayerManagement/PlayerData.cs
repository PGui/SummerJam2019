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
        GameObject player = GameObject.Instantiate(PlayerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject; 
        player.transform.parent = GameObject.Find("PlayerManager").transform;
        player.GetComponent<PlayerController>().playerControllerID = controllerID.ToString();
        playerGameObject = player;
        TeleportAtMenuPosition();
        SceneManager.sceneLoaded += OnSceneLoaded;
        player.GetComponentInChildren<CatCollider>().DelegateChaser += IncreaseScore;
        playerGameObject.GetComponent<CatState>().currentState = eCatState.NONE;
        playerGameObject.GetComponent<CharacterSelector>().HideAllMeshes();
        playerGameObject.GetComponent<CharacterSelector>().InitCharacterWithID(controllerID-1);
    }
    void IncreaseScore(GameObject touchedChaserCat)
    {
        GameObject playerManager = GameObject.Find("PlayerManager");
        if(playerManager != null)
        {
            playerManager.GetComponent<PlayerManager>().IncreaseScore(int.Parse(touchedChaserCat.GetComponent<PlayerController>().playerControllerID));
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerManager.IsMenuScene(scene))
        {
            ToggleReadyImage(false);
            if(playerGameObject.GetComponent<CatState>().currentState == eCatState.NONE)
            {
                playerGameObject.SetActive(false);
            }
            TeleportAtSpawnPosition();
        }
        else
        {
            if(playerGameObject.GetComponent<CatState>().currentState == eCatState.NONE)
            {
                playerGameObject.SetActive(true);
            }
            TeleportAtMenuPosition();
        }
    }
    public void Activate()
    {
        isActive = true;
        if(controllerID == 1)
        {
            playerGameObject.GetComponent<CatState>().currentState = eCatState.CHASER;
        }
        else
        {
            playerGameObject.GetComponent<CatState>().currentState = eCatState.CHASED;
        }
    }
    public void Ready()
    {
        ToggleReadyImage(true);
        isReady = true;
    }
    public void DeReady()
    {
        ToggleReadyImage(false);
        isReady = false;
    }
    public void Deactivate()
    {
        Reset();
    }
    public void TeleportAtMenuPosition()
    {
        float xSpawnPosition = -11+controllerID*2.5f; //-11 is left offset and 2.5 is the space between each cat
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 menuSpawnPos = new Vector3 (xSpawnPosition,0,0);
        cameraPos.y -= 10;
        Vector3 lookAtDir = cameraPos - menuSpawnPos;
        Quaternion menuSpawnRotation = Quaternion.LookRotation(lookAtDir, Vector3.up);

        playerGameObject.transform.position = menuSpawnPos;
        playerGameObject.transform.rotation = menuSpawnRotation;
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
    public void ToggleReadyImage(bool isEnabled)
    {
        GameObject readyImage = playerGameObject.transform.Find("Canvas").transform.Find("Header").transform.Find("Ready").gameObject;
        if(readyImage != null)
        {
            readyImage.SetActive(isEnabled);
        }
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
            playerGameObject.GetComponent<CharacterSelector>().OnDPadRightPressed();
        }
    }
    public void SelectPreviousCharacter()
    {
        if(!isDPadPressed)
        {
            isDPadPressed = true;
            playerGameObject.GetComponent<CharacterSelector>().OnDPadLeftPressed();
        }
    }
    public void SelectNextCharacterColor()
    {
        if(!isDPadPressed)
        {
            isDPadPressed = true;
            playerGameObject.GetComponent<CharacterSelector>().OnDPadUpPressed();
        }
    }
    public void SelectPreviousCharacterColor()
    {
        if(!isDPadPressed)
        {
            isDPadPressed = true;
            playerGameObject.GetComponent<CharacterSelector>().OnDPadDownPressed();
        }
    }
    public void Reset()
    {  
        isActive = false; 
        isReady = false;
        ToggleGameplayControls(false);
        playerGameObject.GetComponent<CharacterSelector>().HideAllMeshes();
        playerGameObject.GetComponent<CatState>().currentState = eCatState.NONE;
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
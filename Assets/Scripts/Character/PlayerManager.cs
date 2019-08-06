using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace LocalCoop {
    /// <summary>
    /// A manager that can be used to add players without having pre-assigned controlled ID's to the input
    /// </summary>
    public class PlayerManager : MonoBehaviour {

        [System.Serializable]
        public class PlayerData
        {    
            public int controllerID = 0;
            public bool isActive = false; //Used to sapwn/unspawn characters
            public bool isReady = false; //Used to set player as "ready" (when all are ready, Load Level scene)
            private bool isDPadPressed = false; //Handle the release of DPad
            public GameObject playerGameObject;
            public void Spawn(GameObject PlayerPrefab)
            {
                //Spawn and store player prefab in GameObject
                //Set the Player ID in PlayerController for input strings matching
                GameObject player = Instantiate(PlayerPrefab, new Vector3 (0,0,0), Quaternion.identity) as GameObject; 
                player.transform.parent = GameObject.Find("PlayerManager").transform;
                player.GetComponent<FakePlayerController>().playerControllerID = controllerID;
                playerGameObject = player;
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
            public void ToggleGameplayControls(bool isEnabled)
            {
                playerGameObject.GetComponent<FakePlayerController>().gameplayControlsEnabled = isEnabled;
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
        }
        
        public static int K_NB_PLAYER_MAX = 8;
        public static string K_HORIZONTAL = "Horizontal_P";
        public static string K_HORIZONTAL_DPAD = "Horizontal_DPad_P";
        public static string K_VERTICAL = "Vertical_P";
        public static string K_JUMP = "Jump_P";
        public static string K_DASH = "Dash_P";        
        public GameObject CharacterSelectionUIPrefab;
        public GameObject PlayerPrefab;
        public List<PlayerData> playerListDyn;


        void Awake() {
            DontDestroyOnLoad(this.gameObject);
        }

        void Start() {
            //Fill PlayerListDyn
            for (int i = 1; i < K_NB_PLAYER_MAX+1; ++i) {
                PlayerData playerData = new PlayerData();
                playerData.controllerID = i;
                playerData.Spawn(PlayerPrefab);
                playerListDyn.Add(playerData);
            }
        }
        
        void Update() {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "CharacterSelection")
            {
                UpdateCharacterSelectionInputsHandling();
            }
            
        }

        void UpdateCharacterSelectionInputsHandling()
        {
            for (int i = 1; i < K_NB_PLAYER_MAX+1; ++i) {
                PlayerData player = FindPlayer(i);
                if(player != null)
                {
                    if(!player.isActive)
                    {
                        if(Input.GetButtonDown(K_JUMP+i.ToString())) //First validation / Character Selection
                        {
                            player.Activate();
                        }
                        else if(Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) == 1)
                        {
                            player.SelectNextCharacter();
                        }
                        else if(Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) == -1)
                        {
                            player.SelectPreviousCharacter();
                        }
                        else if(Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) != -1 && Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) != 1)
                        {
                            player.ReleaseDPad();
                        }
                    }
                    else if(player.isActive)
                    {
                        if(Input.GetButtonDown(K_JUMP+i.ToString()))  //Second validation / Ready to play
                        {
                            player.Ready();
                        }
                        else if(Input.GetButtonDown(K_DASH+i.ToString()))
                        {
                            player.Deactivate();
                        }
                    }
                }
            }
            
            UpdatePlayersValidated();
        }

        void UpdatePlayersValidated() {
            if(GetControllerAmount() == GetPlayerReadyCount())
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene.name == "CharacterSelection")
                {
                    print("All players are readyyyy !!"); //Do send player selection to game level Scene scene = SceneManager.GetActiveScene();
                    LoadLevel("Level0");
                    EnablePlayersGameplayControls();
                }
            }
        }
        void LoadLevel(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
        void EnablePlayersGameplayControls()
        {
            foreach (PlayerData player in playerListDyn)
            {
                if(player.isActive)
                {
                    player.ToggleGameplayControls(true);
                }
            }
        }
        int GetControllerAmount() {
            return Input.GetJoystickNames().Length;
        }

        int GetPlayerActiveCount()
        {
            int activeCount = 0;
            foreach (PlayerData player in playerListDyn)
            {
                if(player.isActive)
                {
                    activeCount++;
                }
            }
            return activeCount;
        }
        int GetPlayerReadyCount()
        {
            int readyCount = 0;
            foreach (PlayerData player in playerListDyn)
            {
                if(player.isReady)
                {
                    readyCount++;
                }
            }
            return readyCount;
        }
        bool IsActivePlayerID(int playerIDToCheck) {
            foreach (PlayerData player in playerListDyn)
            {
                if(player.controllerID == playerIDToCheck)
                {
                    return player.isActive;
                }
            }
            return false;
        }
        PlayerData FindPlayer(int playerControllerID)
        {
            foreach (PlayerData player in playerListDyn)
            {
                if(player.controllerID == playerControllerID)
                {
                    return player;
                }
            }
            return null;
        }
    }
}
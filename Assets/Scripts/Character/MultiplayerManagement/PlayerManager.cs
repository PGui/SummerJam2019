using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum eCatState
{
    CHASER,
    CHASED,
    NONE

}

namespace LocalCoop {
    /// <summary>
    /// A manager that can be used to add players without having pre-assigned controlled ID's to the input
    /// </summary>
    public class PlayerManager : MonoBehaviour {
        public static int K_NB_PLAYER_MAX = 8;
        public static string K_HORIZONTAL = "Horizontal_P";
        public static string K_HORIZONTAL_DPAD = "Horizontal_DPad_P";
        public static string K_VERTICAL = "Vertical_P";
        public static string K_JUMP = "Jump_P";
        public static string K_DASH = "Dash_P";        
        public GameObject CharacterSelectionUIPrefab;
        public GameObject PlayerPrefab;
        public List<PlayerData> playerListDyn;
        public string LevelNameToLoad = "Level0";

        public static PlayerManager singleton = null;

        public static bool IsMenuScene(Scene scene)
        {
            return scene.name == "CharacterSelection";
        }
        void Awake() {
            //Check if instance already exists
            if (singleton == null) {
                //if not, set instance to this
                singleton = this;
            }

            //If instance already exists and it's not this:
            else if (singleton != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }

        void Start() {
            CreatePlayers();
        }

        void CreatePlayers()
        {
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
            if (IsMenuScene(scene))
            {
                UpdateCharacterSelectionInputsHandling();
            }
            else
            {
                if(AllCatsAreChasers())
                {
                    print("END OF GAME");
                    //DestroyPlayers();
                    //DestroyImmediate(this.gameObject);
                    ResetPlayers();
                    LoadLevel("CharacterSelection");
                }
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
                        if(!player.isReady)
                        {
                            if(Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) == 1)
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
                    }
                }
            }
            
            UpdatePlayersValidated();
        }

        void UpdatePlayersValidated() {
            if(GetControllerAmount() == GetPlayerReadyCount())
            {
                Scene scene = SceneManager.GetActiveScene();
                if (IsMenuScene(scene))
                {
                    print("All players are readyyyy !!"); //Do send player selection to game level Scene scene = SceneManager.GetActiveScene();
                    LoadLevel(LevelNameToLoad);
                    InitPlayersAfterLoadLevel();
                }
            }
        }
        void LoadLevel(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }
        void InitPlayersAfterLoadLevel()
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
        bool AllCatsAreChasers()
        {
            int chaserCatsCount = 0;
            foreach (PlayerData player in playerListDyn)
            {
                if(player.playerGameObject.GetComponent<CatState>().currentState == eCatState.CHASER)
                {
                    chaserCatsCount++;
                }
            }
            return chaserCatsCount == GetPlayerReadyCount();
        }
        public void IncreaseScore(int playerControllerID)
        {
            PlayerData player = FindPlayer(playerControllerID);
            if(player != null)
            {
                player.score++;
            }
        }        
        void ResetPlayers()
        {
            foreach (PlayerData player in playerListDyn)
            {
                player.Reset();
            }
        }
        void DestroyPlayers()
        {
            foreach (PlayerData player in playerListDyn)
            {
                player.OnDisable();
            }
            playerListDyn.Clear();
        }
    }
}
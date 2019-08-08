using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        public static string K_VERTICAL_DPAD = "Vertical_DPad_P";
        public static string K_VERTICAL = "Vertical_P";
        public static string K_JUMP = "Jump_P";
        public static string K_DASH = "Dash_P";        
        public GameObject CharacterSelectionUIPrefab;
        public GameObject PlayerPrefab;
        public List<PlayerData> playerListDyn;
        public string LevelNameToLoad = "Level0";
        public GameObject UICharacterSelection;
        public GameObject UIVictoryScreen;
        public GameObject UICountDown;

        [Header("CountDown")]
        public float startCountdown = 3;
        public string startMessage = "GO !";
        private int displayedCountdown = 0;

        public static PlayerManager singleton = null;
      

        // Sounds
        AudioSource audioSource;
        public AudioClip[] StartSounds;

       

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
            CreateAudioSource();
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

                if(startCountdown > 0)
                {
                    startCountdown -= Time.deltaTime;
                    int previousCountdown = displayedCountdown;
                    displayedCountdown = Mathf.RoundToInt(startCountdown);

                    if(displayedCountdown < previousCountdown)
                    {
                        if(displayedCountdown == 0) {
                            UpdateCountDown(startMessage, 2);//index for 0/GO  sound
                            InitPlayersAfterLoadLevel();
                        }
                        else UpdateCountDown(displayedCountdown.ToString(), 0);//index for 3 2 1 sounds
                    }
                    
                    if(startCountdown <= 0){
                       UICountDown.SetActive(false);
                    }
                }
                else if(AllCatsAreChasers())
                {
                    if(!UIVictoryScreen.activeInHierarchy)
                    {
                        print("END OF GAME");
                        GameObject fireworkContainer = GameObject.Find("FireworkFXsWinner");
                        if(fireworkContainer != null)
                        {
                            PlayerData winner = FindWinner();
                            Color fireworkColor = Color.red;
                            if(winner != null)
                            {
                                fireworkColor = winner.GetCatColor();
                            }
                            ParticleSystem[] fxArray = fireworkContainer.GetComponentsInChildren<ParticleSystem>();
                            foreach(ParticleSystem sys in fxArray)
                            {
                                var main = sys.main;
                                main.startColor = fireworkColor;
                                sys.Play();
                            }
                        }
                        GameObject fireworkContainerColored = GameObject.Find("FireworkFXsColored");
                        if(fireworkContainerColored != null)
                        {
                            ParticleSystem[] fxArray = fireworkContainerColored.GetComponentsInChildren<ParticleSystem>();
                            foreach(ParticleSystem sys in fxArray)
                            {
                                sys.Play();
                            }
                        }
                        UIVictoryScreen.SetActive(true);
                    }
                    if(Input.GetButtonDown("StartEndGame"))
                    {
                        ResetPlayers();
                        UIVictoryScreen.SetActive(false);
                        UICharacterSelection.SetActive(true);
                        LoadLevel("CharacterSelection");
                    }
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
                            if(player.isReady)
                            {
                                player.DeReady();
                            }
                            else
                            {
                                player.Deactivate();
                            }
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
                            else if(Input.GetAxis(K_VERTICAL_DPAD+i.ToString()) == 1)
                            {
                                player.SelectNextCharacterColor();
                            }
                            else if(Input.GetAxis(K_VERTICAL_DPAD+i.ToString()) == -1)
                            {
                                player.SelectPreviousCharacterColor();
                            }
                            else if(Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) != -1 && Input.GetAxis(K_HORIZONTAL_DPAD+i.ToString()) != 1 
                                    && Input.GetAxis(K_VERTICAL_DPAD+i.ToString()) != -1 && Input.GetAxis(K_VERTICAL_DPAD+i.ToString()) != 1)
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
                    UICharacterSelection.SetActive(false);
                    LoadLevel(LevelNameToLoad);
                              
                    //InitPlayersAfterLoadLevel();
                }
            }
        }
       
        void LoadLevel(string sceneName) {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += OnLevelLoaded;
        }
        
        void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            if(!IsMenuScene(scene))
                UICountDown.SetActive(true);  
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

        void UpdateCountDown(string message, int count)
        {
            UICountDown.GetComponent<Animator>().SetTrigger("PlayBump");
            PlayReadySound(count);
            UICountDown.GetComponent<Text>().text = message;
            
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
        public bool IsActivePlayerID(int playerIDToCheck) {
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
                player.playerGameObject.GetComponentInChildren<CatCanvasFollow>().score.text = player.score.ToString();
            }
        }
        public PlayerData FindWinner()
        {
            int bestScore = 0;
            PlayerData playerWinner = null;
            foreach (PlayerData player in playerListDyn)
            {
                if(player.score >= bestScore)
                {
                    bestScore = player.score;
                    playerWinner=player;
                }
            }
            return playerWinner;
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

        void CreateAudioSource()
        {
            audioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        }

        void PlayReadySound(int index)
        {
            audioSource.PlayOneShot(StartSounds[index]);
        }
    }

    
}
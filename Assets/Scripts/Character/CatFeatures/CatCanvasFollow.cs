using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using LocalCoop;

public class CatCanvasFollow : MonoBehaviour
{

    public Text playerName;
    public Text countdown;
    public Text score;
    public GameObject header;
    public GameObject readyImage;
    public Vector3 offset = new Vector3(0,15,0);
    [Header("Player Name")]
    public string playersName = "Cat";
    public float hideNameCountdown = 10;
   
    private GameObject player;
    private CatEnergy catEnergy;
    private Scene currentScene;
      // Start is called before the first frame update
    void Start()
    {
        player = this.transform.parent.gameObject;

        playerName.text = playersName + " " + player.GetComponent<PlayerController>().playerControllerID;
        
        player.GetComponentInChildren<CatCollider>().DelegateChaser += OnBecameChaser;
        SceneManager.sceneLoaded += OnSceneLoaded;
        catEnergy = player.GetComponent<CatEnergy>();

        countdown.gameObject.SetActive(false);
        //playerName = header.transform.Find("Name").GetComponent<Text>();
       //countdown = header.transform.Find("Countdown").GetComponent<Text>();
         //score = header.transform.Find("Score").GetComponent<Text>();           
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerManager.IsMenuScene(scene))
        {
            if(player.GetComponent<CatState>().currentState == eCatState.CHASER)
            {
                score.gameObject.SetActive(true);
            }
      

        }
        else
        {
            score.gameObject.SetActive(false);
        }
    }

    void OnBecameChaser(GameObject touchingChaser)
    {
        score.gameObject.SetActive(true);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        header.transform.LookAt(Camera.main.transform);

        playerName.color = player.GetComponent<CharacterSelector>().GetCatColor();

        if(catEnergy != null){


            float count = Mathf.Round( catEnergy.energyCountdown * 100f) / 100f;
            countdown.text = count.ToString();
        }

        if(hideNameCountdown > 0 && currentScene != null && !PlayerManager.IsMenuScene(currentScene))
        {
            hideNameCountdown -= Time.deltaTime;
            if(hideNameCountdown <= 0) 
            {
                playerName.gameObject.SetActive(false);
                DisplayReadyImage(readyImage.GetComponent<Image>().color, false);
            }
        }
                  
    }
    public void DisplayReadyImage(Color color, bool active)
    {
        readyImage.SetActive(active);
        readyImage.GetComponent<Image>().color = color;
    }
    public void TriggerEmote()
    {
        hideNameCountdown = 0.25f;
        DisplayReadyImage(readyImage.GetComponent<Image>().color, true);
    }
}

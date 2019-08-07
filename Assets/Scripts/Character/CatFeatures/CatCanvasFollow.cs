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
    public string playersName = "Cat";
    public GameObject header;
    private GameObject player;
    private CatEnergy catEnergy;
    public Vector3 offset = new Vector3(0,20,0);
    
  
      // Start is called before the first frame update
    void Start()
    {
        player = this.transform.parent.gameObject;

        playerName.text = playersName + " " + player.GetComponent<PlayerController>().playerControllerID;
        
        player.GetComponentInChildren<CatCollider>().DelegateChaser += OnBecameChaser;
        SceneManager.sceneLoaded += OnSceneLoaded;
        catEnergy = player.GetComponent<CatEnergy>();
        //playerName = header.transform.Find("Name").GetComponent<Text>();
       //countdown = header.transform.Find("Countdown").GetComponent<Text>();
         //score = header.transform.Find("Score").GetComponent<Text>();           
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!PlayerManager.IsMenuScene(scene))
        {
            if(player.GetComponent<CatState>().currentState == eCatState.CHASED)
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
    void Update()
    {
        Vector3 uiPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        header.transform.position = uiPosition + offset;

        playerName.color = player.GetComponent<CharacterSelector>().GetCatColor();

        if(catEnergy != null){


            float count = Mathf.Round( catEnergy.energyCountdown * 100f) / 100f;
            countdown.text = count.ToString();
        }
            
    }
}

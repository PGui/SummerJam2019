using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{

    public Text playerName;
    public Text countdown;

    public GameObject player;

    private Camera gameCamera;
  
      // Start is called before the first frame update
    void Start()
    {
        gameCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 uiPosition = gameCamera.WorldToScreenPoint(player.transform.position);
        playerName.transform.position = uiPosition;

        countdown.text = player.GetComponent<CatEnergy>().energyCountdown.ToString();
        countdown.transform.position = uiPosition;
    }
}

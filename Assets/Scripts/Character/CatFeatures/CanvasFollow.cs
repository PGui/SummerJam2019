﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasFollow : MonoBehaviour
{

    private Text playerName;
    private Text countdown;
    private Text score;
    public GameObject header;
    private GameObject player;
    private CatEnergy catEnergy;
    private Camera gameCamera;
    
  
      // Start is called before the first frame update
    void Start()
    {
        gameCamera = Camera.main;
        player = this.transform.parent.gameObject;

        catEnergy = player.GetComponent<CatEnergy>();
        //playerName = header.transform.Find("Name").GetComponent<Text>();
       //countdown = header.transform.Find("Countdown").GetComponent<Text>();
         //score = header.transform.Find("Score").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 uiPosition = gameCamera.WorldToScreenPoint(player.transform.position);
        header.transform.position = uiPosition;

        if(catEnergy != null)
            countdown.text = catEnergy.energyCountdown.ToString();
    }
}

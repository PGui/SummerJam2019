using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CatEnergy : MonoBehaviour
{

    public float speedDivider = 2f;
    public float energyCountdown = 10f;
    private float baseGroundSpeed;
    private float baseAirSpeed;
    private float baseDashDistance;
    PlayerController playerController;

    public Text countDown;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        baseGroundSpeed =  playerController.groundSpeed;
        baseAirSpeed = playerController.airSpeed;
        baseDashDistance = playerController.dashDistance;

    }

    // Update is called once per frame
    void Update()
    {

        if(energyCountdown > 0)
        {
            energyCountdown -= Time.deltaTime;
            if(energyCountdown <= 0){
                energyCountdown = 0;
                playerController.groundSpeed /= speedDivider;
                playerController.airSpeed /= speedDivider;
                playerController.dashDistance /= speedDivider;
                
            }
        }
        
    }

    public void RefillTime(float time)
    {
        if(time > 0)
        {
            if(energyCountdown == 0 && time > 0)
            {
                playerController.groundSpeed *= speedDivider;
                playerController.airSpeed *= speedDivider;
                playerController.dashDistance *= speedDivider;
            }

            energyCountdown += time;
        }
        


    }

   
}

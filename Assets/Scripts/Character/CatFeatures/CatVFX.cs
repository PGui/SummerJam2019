using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatVFX : MonoBehaviour
{
    private PlayerController playerController;
    public ParticleSystem walkVFX;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnMove += OnMove;
        playerController.OnStop += OnStop;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMove()
    {
        if (!walkVFX.isPlaying)
        {
            walkVFX.Play();
        }
        
    }

    void OnStop()
    {
        if (walkVFX.isPlaying)
        {
            walkVFX.Stop();
        }
    }
}

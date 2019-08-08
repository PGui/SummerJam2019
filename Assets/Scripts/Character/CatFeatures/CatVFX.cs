using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatVFX : MonoBehaviour
{
    private PlayerController playerController;
    public ParticleSystem walkVFX;
    public ParticleSystem fightVFX;

    ShakeableTransform cameraShake;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnMove += OnMove;
        playerController.OnStop += OnStop;
        this.GetComponentInChildren<CatCollider>().DelegateChaser += StartFightVFX;

        GameObject shakeGameobject = GameObject.FindGameObjectWithTag("CameraShake");
        cameraShake = shakeGameobject?.GetComponent<ShakeableTransform>();
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
            // if(GetComponent<CatState>().currentState == eCatState.CHASER) //If we want the black cats to have black smoke
            // {
            //     var main = walkVFX.main;
            //     main.startColor = Color.black;
            // }
            var main = walkVFX.main;
            main.startColor = GetComponent<CharacterSelector>().GetCatColor();
        }
        
    }

    void OnStop()
    {
        if (walkVFX.isPlaying)
        {
            walkVFX.Stop();
        }
    }

    void StartFightVFX(GameObject touchingChaser)
    {
        if (fightVFX != null && !fightVFX.isPlaying)
        {
            fightVFX.Play();
            cameraShake.ResetToDefault();
            cameraShake.maximumAngularShake = new Vector3(15,15,15);
            cameraShake.recoverySpeed = 0.3f;
            cameraShake.frequency = 5.0f;
            cameraShake.traumaExponent = 1.53f;
            cameraShake.InduceStress(0.3f);
            
        }
    }
}

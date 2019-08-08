using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperZone : MonoBehaviour
{
    public AudioClip[] jumperZoneClips;
    
    public float jumpMultiplier = 1.5f;

    AudioSource audioSource;

    ShakeableTransform cameraShake;
    // Start is called before the first frame update

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameObject shakeGameobject = GameObject.FindGameObjectWithTag("CameraShake");
        cameraShake = shakeGameobject?.GetComponent<ShakeableTransform>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.jumpMultiplier*= jumpMultiplier;

            audioSource.PlayOneShot(jumperZoneClips[Random.Range(0, jumperZoneClips.Length)]);

            cameraShake.ResetToDefault();
            // cameraShake.recoverySpeed = 0.1f;
            cameraShake.frequency = 25.0f;
            cameraShake.traumaExponent = 0.8f;
            cameraShake?.InduceStress(0.01f);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.jumpMultiplier/= jumpMultiplier;
        }
    }
}

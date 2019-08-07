using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSFX : MonoBehaviour
{
    AudioSource audioSource;
    AudioSource walkAudioSource;

    public AudioClip[] walkSounds;

    public AudioClip jumpSound;
    public AudioClip dashSound;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnJump += OnJump;
        playerController.OnDash += OnDash;
        playerController.OnMove += OnMove;

        audioSource = GetComponent<AudioSource>();

        walkAudioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
    }

    void OnDash()
    {
        audioSource.PlayOneShot(dashSound, 0.5f);
    }

    void OnJump()
    {
        audioSource.PlayOneShot(jumpSound, 0.5f);
    }

    void OnMove()
    {
        if (!walkAudioSource.isPlaying)
        {
            print(Random.Range(0, walkSounds.Length));
            walkAudioSource.PlayOneShot(walkSounds[Random.Range(0, walkSounds.Length)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerController.)
    }
}

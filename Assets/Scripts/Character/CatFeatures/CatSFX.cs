using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSFX : MonoBehaviour
{
    AudioSource audioSource;
    AudioSource walkAudioSource;
    AudioSource angryAudioSource;

    public AudioClip[] walkSounds;

    public AudioClip[] angrySounds;

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
        angryAudioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;

        this.GetComponentInChildren<CatCollider>().DelegateChaser += OnCatCatched;
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
            //print(Random.Range(0, walkSounds.Length));
            walkAudioSource.PlayOneShot(walkSounds[Random.Range(0, walkSounds.Length)]);
        }
    }

    void OnCatCatched(GameObject cat)
    {
        if (!angryAudioSource.isPlaying && angrySounds.Length > 0)
        {
            //print(Random.Range(0, walkSounds.Length));
            angryAudioSource.PlayOneShot(angrySounds[Random.Range(0, angrySounds.Length)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerController.)
    }
}

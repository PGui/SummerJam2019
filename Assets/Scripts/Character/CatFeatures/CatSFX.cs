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

    public AudioClip[] catSounds;

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
            angryAudioSource.PlayOneShot(angrySounds[Random.Range(0, angrySounds.Length)]);
        }
    }

    public void PlayReadySound()
    {
        if (catSounds.Length > 0)
        {
            float previousVolume = audioSource.volume;
            float previousMinDistance = audioSource.minDistance;
            float previousMaxDistance = audioSource.maxDistance;
            audioSource.minDistance = 990.0f;
            audioSource.maxDistance = 1000.0f;
            audioSource.volume = 1.0f;
            audioSource.PlayOneShot(catSounds[Random.Range(0, catSounds.Length)]);
            audioSource.volume = previousVolume;
            audioSource.minDistance = previousMinDistance;
            audioSource.maxDistance = previousMaxDistance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (playerController.)
    }
}

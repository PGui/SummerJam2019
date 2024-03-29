﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] pickUpClips;
   
    private static int activePickupCount = 0;
    private static List<Pickup> pickups = new List<Pickup>();
     [Header("Pickup")]
    public float timeRefill = 5f;
    public bool canBePicked = true;

    [Header("Reactivation")]
    public bool reactivateAfterDelay = false;
    public float reactivationDelay = 5f;

    private float reactivationTimer = 0f;

    [Header("VFX")]
    public GameObject pickupVfx;

    private void Awake()
    {
        pickups.Add(this);
        activePickupCount++;
    
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(reactivateAfterDelay)
        {
            reactivationTimer += Time.deltaTime;
            if(reactivationTimer > reactivationDelay)   Activate();
        }
        
    }

    void Activate()
    {
        canBePicked = true;
        activePickupCount++;
        GetComponentInParent<Renderer>().enabled = true;
        GetComponentInChildren<Light>().enabled = true;
    }

    void DeActivate()
    {
        canBePicked = false;
        activePickupCount--;
        GetComponentInParent<Renderer>().enabled = false;
        GetComponentInChildren<Light>().enabled = false;
        if(activePickupCount == 0)
        {
            foreach (Pickup pickup in pickups)
            {
                if(pickup != this)
                {
                    pickup.Activate();
                }
            }
        }

        reactivationTimer = 0;

        audioSource.PlayOneShot(pickUpClips[Random.Range(0, pickUpClips.Length)]);
        
        //Spawn vfx
        GameObject vfxGameObject = GameObject.Instantiate<GameObject>(pickupVfx,this.transform.position, this.transform.rotation);
        vfxGameObject.GetComponent<ParticleSystem>().Play();
        Destroy(vfxGameObject, 3.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
       
        if ( !canBePicked || other.gameObject.tag != "TouchCollider")
            return;

        GameObject otherEntity = other.transform.parent.gameObject;
        eCatState otherState = otherEntity.GetComponent<CatState>().currentState;        
        if (otherState == eCatState.CHASED)
        {
            CatEnergy energy =    otherEntity.GetComponent<CatEnergy>();
            energy.RefillTime(timeRefill);
            DeActivate();
        }
    }
}

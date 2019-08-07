using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {
 
public static float Map (this float value, float from1, float to1, float from2, float to2) {
    return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
}
   
}

public class CameraManager : MonoBehaviour
{
    [ReadOnly] public GameObject[] Players;

    public Vector2 rotationXRange = new Vector2(27, 33);
    public Vector2 rotationYRange = new Vector2(-3, 3);

    public Vector3 targetPosition;

    public float minFov = 25.0f;
    public float maxFov = 30.0f;

    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        Players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(Players);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 min = new Vector3(100000, 100000, 100000);
        Vector3 max = new Vector3(-100000, -100000, -10000);
        // Compute the target position
        foreach (GameObject go in Players)
        {
            if(go.GetComponent<CatState>().currentState == eCatState.NONE)
            {
                continue;
            }
            min.x = Mathf.Min(min.x, go.transform.position.x);
            min.y = Mathf.Min(min.y, go.transform.position.y);
            min.z = Mathf.Min(min.z, go.transform.position.z);

            max.x = Mathf.Max(max.x, go.transform.position.x);
            max.y = Mathf.Max(max.y, go.transform.position.y);
            max.z = Mathf.Max(max.z, go.transform.position.z);
        }

        targetPosition = (max + min) / 2.0f;

        Vector3 targetVector = (targetPosition - this.transform.position).normalized;
        
        this.transform.forward = Vector3.SmoothDamp(this.transform.forward, targetVector, ref velocity, 0.5f);

        //Compute the cat's distance
        Vector3 extend = (max - min) / 2.0f;
        float maxDistance = Mathf.Max(extend.x, extend.y, extend.z);

        float currentFov = maxFov;
        float unclampedRatio = maxDistance/20.0f;
        if (unclampedRatio < 0.6f)
        {
            float ratio = unclampedRatio.Map(0.0f, 0.3f, 0.0f, 1.0f);

            currentFov = Mathf.Lerp(minFov, maxFov, ratio);

            // Debug.Log(currentFov);
        }

        this.GetComponent<Camera>().fieldOfView =  Mathf.SmoothStep(this.GetComponent<Camera>().fieldOfView, currentFov, 0.2f);
        
    }
}

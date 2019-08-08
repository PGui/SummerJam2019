using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LocalCoop;

public class CatAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
    bool ready = false;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerController.OnMove += OnMove;
        playerController.OnStop += OnStop;
        playerController.OnJump += OnJump;
        playerController.OnDash += OnDash;
        playerController.OnJumpStop += OnJumpStop;
        playerController.OnDashStop += OnDashStop;
    }

    // Update is called once per frame
    void Update()
    {
        if(animator != null && animator.GetBool("IsDashing"))
        {
            if(!animator.IsInTransition(0))
            {
                print(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f &&
                    animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.8f &&
                    !animator.IsInTransition(0))
            {
                OnDashStop();
            }
        }
    }
    void ToggleAnimparam(string paramName, bool paramValue)
    {
        if(GetComponent<PlayerController>().gameplayControlsEnabled)
        {
            animator = GetComponent<CharacterSelector>().GetCurrentMesh().GetComponent<Animator>();
            animator?.SetBool(paramName, paramValue);
        }
    }
    void OnMove()
    {
        ToggleAnimparam("IsMoving", true);
    }
    void OnStop()
    {
        ToggleAnimparam("IsMoving", false);
    }

    void OnJump()
    {
        ToggleAnimparam("IsJumping", true);
    }
    void OnDash()
    {
        print("DASH");
        ToggleAnimparam("IsDashing", true);
    }
    void OnJumpStop()
    {
        ToggleAnimparam("IsJumping", false);
    }
    void OnDashStop()
    {
        print("DASHSTOP");
        ToggleAnimparam("IsDashing", false);
    }
}

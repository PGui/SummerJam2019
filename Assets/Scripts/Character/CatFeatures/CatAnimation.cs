using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerController playerController;
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
        animator = GetComponent<CharacterSelector>().GetCurrentMesh().GetComponent<Animator>();
        if(animator != null && animator.GetBool("IsDashing"))
        {
            OnDashStop();
        }
    }

    void OnMove()
    {
        animator?.SetBool("IsMoving", true);
    }
    void OnStop()
    {
        animator?.SetBool("IsMoving", false);
    }

    void OnJump()
    {
        animator?.SetBool("IsJumping", true);
    }
    void OnDash()
    {
        animator?.SetBool("IsDashing", true);
    }
    void OnJumpStop()
    {
        animator?.SetBool("IsJumping", false);
    }
    void OnDashStop()
    {
        animator?.SetBool("IsDashing", false);
    }
}

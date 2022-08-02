using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for movement input
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("isWalking", true);
            if (Input.GetAxis("Horizontal") != 0)
            {
                animator.SetFloat("yInput", 0);
                animator.SetFloat("xInput", Input.GetAxis("Horizontal"));
            }
            else
            {
                animator.SetFloat("xInput", 0);
                animator.SetFloat("yInput", Input.GetAxis("Vertical"));
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float jumpForce;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Jumppad");

        if (collision.gameObject.TryGetComponent(out Rigidbody2D rb) && collision.gameObject.TryGetComponent(out CinemachineImpulseSource impulseSource))
        {
            rb.AddForce(transform.up * jumpForce);
            impulseSource.GenerateImpulse();
        }

    }
}


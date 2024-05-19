using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeClimb : MonoBehaviour
{
    private Animator animator;

    //CapsuleCollider bileseni
    private CapsuleCollider2D capsuleCollider2d;
    [SerializeField] LayerMask ledgeLayerMask;

    private Movement movement;
    private Jump jump;

    //Duvar kontrol degiskeni
    private bool isTouchingWall;
    [HideInInspector] public bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private bool canLedge = false;

    private Rigidbody2D rb;

    // Baslangic metodu - Oyun basladiginda bir kere çalisir
    void Start()
    {
        animator = GetComponent<Animator>(); //Animator Caching
        capsuleCollider2d = GetComponent<CapsuleCollider2D>(); //CapsuleCollider Caching
        movement = GetComponent<Movement>();
        jump = GetComponent<Jump>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PerformWallSliding();
    }

    void FixedUpdate()
    {
        CheckLedges();
        CheckWallSliding();
    }

    void CheckWallSliding()
    {
        RaycastHit2D raycastHitCenter = Physics2D.Raycast(new Vector2(capsuleCollider2d.bounds.center.x, capsuleCollider2d.bounds.center.y), Vector2.right * movement.rayDirection, 0.4f, jump.groundlayerMask);
        RaycastHit2D raycastHitTop = Physics2D.Raycast(new Vector2(capsuleCollider2d.bounds.center.x, capsuleCollider2d.bounds.max.y), Vector2.right * movement.rayDirection, 0.4f, jump.groundlayerMask);

        isTouchingWall = raycastHitTop.collider != null && raycastHitCenter.collider != null;

        if (isTouchingWall && !jump.isGrounded && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }
    void PerformWallSliding()
    {
        if (isWallSliding)
        {
            if (rb.velocity.y < wallSlidingSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlidingSpeed);
            }
        }
    }
    void CheckLedges()
    {
        RaycastHit2D raycastHitTop = Physics2D.Raycast(new Vector2(capsuleCollider2d.bounds.center.x, capsuleCollider2d.bounds.max.y), Vector2.right * movement.rayDirection, 1f, ledgeLayerMask);

        Color rayColorTop;

        if (raycastHitTop.collider != null)
        {
            rayColorTop = Color.green;
        }
        else
        {
            rayColorTop = Color.red;
        }

        Debug.DrawRay(new Vector2(capsuleCollider2d.bounds.center.x, capsuleCollider2d.bounds.max.y), Vector2.right * movement.rayDirection * 1f, rayColorTop);

        RaycastHit2D raycastHitCenter = Physics2D.Raycast(new Vector2(capsuleCollider2d.bounds.center.x, capsuleCollider2d.bounds.center.y), Vector2.right * movement.rayDirection, 1f, ledgeLayerMask);

        Color rayColorCenter;

        if (raycastHitCenter.collider != null)
        {
            rayColorCenter = Color.green;
        }
        else
        {
            rayColorCenter = Color.red;
        }

        Debug.DrawRay(new Vector2(capsuleCollider2d.bounds.center.x, capsuleCollider2d.bounds.center.y), Vector2.right * movement.rayDirection * 1f, rayColorCenter);

        //Tepedeki ray boþta ve merkezdeki ray collidera deðiyorsa ledge climb yap
        if (raycastHitTop.collider == null && raycastHitCenter.collider != null && !jump.isGrounded)
        {
            canLedge = true;
            PerformLedgeClimb();
        }
    }
    void PerformLedgeClimb()
    {
        if (canLedge && Input.GetAxisRaw("Horizontal") != 0 && Input.GetButtonDown("Jump"))
        {
            Vector2 ledgeClimbPosition = new Vector2(capsuleCollider2d.bounds.center.x + movement.rayDirection, capsuleCollider2d.bounds.max.y);
            transform.position = ledgeClimbPosition;
        }
    }
}
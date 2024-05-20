using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Jump : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2d;
    private Animator animator;
    public LayerMask groundlayerMask;
    [SerializeField] LayerMask mattressLayerMask;
    public bool isGrounded;
    private bool hasJumped;
    private float doubleJumpCount;
    [SerializeField] private float maxDoubleJumps;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float maxJumpTime;
    private bool canJump = true;
    private bool isJumping;
    private float jumpTime;
    private Rigidbody2D rb;
    [SerializeField] private float jumpForceMin;
    [SerializeField] private float jumpForceMax;
    [SerializeField] private float backJumpForce = 5f;

    public GameOver gameover;

    private CinemachineImpulseSource impulseSource;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        capsuleCollider2d = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckGrounded();
        CheckJumpInput();
        CheckFalling();
    }
    void CheckGrounded()
    {
        // Karakterin yerde olup olmadýðýný kontrol et
        RaycastHit2D raycastHit = Physics2D.Raycast(capsuleCollider2d.bounds.center, Vector2.down, capsuleCollider2d.bounds.extents.y + 0.5f, groundlayerMask);

        Color rayColor;

        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        Debug.DrawRay(capsuleCollider2d.bounds.center, Vector2.down * (capsuleCollider2d.bounds.extents.y + 0.5f), rayColor);
        isGrounded = raycastHit.collider != null;


        // Karakter yerdeyse, zýplama durumunu sýfýrla
        if (isGrounded)
        {
            hasJumped = false;
            doubleJumpCount = 0;
        }

    }

    void CheckJumpInput()
    {
        // "Jump" tuþuna basýldýðýnda
        if (Input.GetButtonDown("Jump") && canJump)
        {
            // Eðer yerdeyse
            if (isGrounded)
            {
                // Zýplama baþlat
                isJumping = true;
                jumpTime = 0f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForceMin);
                animator.SetBool("isJumping", true);
            }
            // Yerde deðilse ve çift zýplama kullanýlabilirse
            else if (!hasJumped && doubleJumpCount < maxDoubleJumps)
            {
                // Çift zýplama baþlat
                hasJumped = true;
                doubleJumpCount++;
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                animator.SetBool("isJumping", true);
            }
        }

        // "Jump" tuþuna basýlý tutulduðu sürece ve zýplama zaman sýnýrýna ulaþýlmamýþsa
        if (Input.GetButton("Jump") && isJumping && jumpTime < maxJumpTime && canJump)
        {
            jumpTime += Time.deltaTime;
            float jumpForce = Mathf.Lerp(jumpForceMin, jumpForceMax, jumpTime / maxJumpTime); // Lineer Interpolasyon
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool("isJumping", true);
        }
        else
        {
            if (isGrounded)
            {
                animator.SetBool("isJumping", false);
            }
        }

        // "Jump" tuþu býrakýldýðýnda
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTime = 0f;
            animator.SetBool("isJumping", false);
        }

        // "BackJump" tuþuna basýldýðýnda
        if (Input.GetButtonDown("BackJump") && canJump && isGrounded)
        {
            // Geriye doðru zýplama baþlat
            rb.velocity = new Vector2(-backJumpForce, rb.velocity.y);
            animator.SetBool("isJumping", true);
        }
        else
        {
            if (isGrounded)
            {
                animator.SetBool("isJumping", false);
            }
        }
    }
    void CheckFalling()
    {
        if (rb.velocity.y < -7f)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
        }

        if (rb.velocity.y < -10f)
        {
            RaycastHit2D mattressHit = Physics2D.Raycast(capsuleCollider2d.bounds.center, Vector2.down, capsuleCollider2d.bounds.extents.y + 0.6f, mattressLayerMask);

            if (mattressHit.collider != null)
            {
                animator.SetTrigger("isGrounded");
                animator.SetBool("isFalling", false);
                impulseSource.GenerateImpulse();
            } else if (isGrounded)
            {
                animator.SetTrigger("isGrounded");
                animator.SetTrigger("Hurt");
                impulseSource.GenerateImpulse();
                gameover.isOver = true;
            }
        }
    }
}

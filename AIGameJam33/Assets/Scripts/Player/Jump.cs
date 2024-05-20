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
        // Karakterin yerde olup olmad���n� kontrol et
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


        // Karakter yerdeyse, z�plama durumunu s�f�rla
        if (isGrounded)
        {
            hasJumped = false;
            doubleJumpCount = 0;
        }

    }

    void CheckJumpInput()
    {
        // "Jump" tu�una bas�ld���nda
        if (Input.GetButtonDown("Jump") && canJump)
        {
            // E�er yerdeyse
            if (isGrounded)
            {
                // Z�plama ba�lat
                isJumping = true;
                jumpTime = 0f;
                rb.velocity = new Vector2(rb.velocity.x, jumpForceMin);
                animator.SetBool("isJumping", true);
            }
            // Yerde de�ilse ve �ift z�plama kullan�labilirse
            else if (!hasJumped && doubleJumpCount < maxDoubleJumps)
            {
                // �ift z�plama ba�lat
                hasJumped = true;
                doubleJumpCount++;
                rb.velocity = new Vector2(rb.velocity.x, doubleJumpForce);
                animator.SetBool("isJumping", true);
            }
        }

        // "Jump" tu�una bas�l� tutuldu�u s�rece ve z�plama zaman s�n�r�na ula��lmam��sa
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

        // "Jump" tu�u b�rak�ld���nda
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTime = 0f;
            animator.SetBool("isJumping", false);
        }

        // "BackJump" tu�una bas�ld���nda
        if (Input.GetButtonDown("BackJump") && canJump && isGrounded)
        {
            // Geriye do�ru z�plama ba�lat
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

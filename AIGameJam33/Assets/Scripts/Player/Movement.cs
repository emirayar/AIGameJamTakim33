using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isFacingRight;
    [HideInInspector] public float rayDirection;
    [HideInInspector] public Vector2 movement;
    private SlideController slideController;
    private LedgeClimb ledgeClimb;
    private Jump jump;

    // Ses ekleme için gerekli deðiþkenler
    [SerializeField] private AudioClip footstepSound;
    private AudioSource audioSource;
    private bool isPlayingFootstep;
    public ClapperBoard clapperBoard;
    public GameOver gameOver;
    public NextLevel next;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        slideController = GetComponent<SlideController>();
        ledgeClimb = GetComponent<LedgeClimb>();
        jump = GetComponent<Jump>();

        // AudioSource bileþenini al
        audioSource = GetComponent<AudioSource>();
        isPlayingFootstep = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (slideController.isGroundSliding || clapperBoard.countdown || gameOver.isOver)
        {
            return;
        }
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (ledgeClimb.isWallSliding)
        {
            movement = new Vector2(Input.GetAxis("Horizontal") * 0f, rb.velocity.y);
            rb.velocity = movement;
        }
        // Karakterin yüzünü çevir
        FlipCharacter(horizontalInput);

        // Adým sesini çal
        PlayFootstepSound(Mathf.Abs(rb.velocity.x));
    }

    // Karakteri çevirme metodu
    void FlipCharacter(float horizontalInput)
    {
        // Karakterin yüzünü çevirme
        if ((horizontalInput < 0 && isFacingRight) || (horizontalInput > 0 && !isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    void CheckRayDirection()
    {
        // Ray yönünü güncelle
        rayDirection = isFacingRight ? 1f : -1f;
    }

    void PlayFootstepSound(float speed)
    {
        if (speed > 2f && !isPlayingFootstep && jump.isGrounded)
        {
            audioSource.clip = footstepSound;
            audioSource.Play();
            isPlayingFootstep = true;
        }
        else if (speed <= 2f)
        {
            audioSource.Stop();
            isPlayingFootstep = false;
        }
    }
}

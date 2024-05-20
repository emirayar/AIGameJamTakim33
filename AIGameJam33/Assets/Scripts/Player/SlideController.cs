using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Movement movement;
    private Jump jump;
    private CapsuleCollider2D capsuleCollider;
    public CapsuleCollider2D slideCollider;
    private Animator animator;

    public bool isGroundSliding = false;

    private bool canSlide = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = GetComponent<Jump>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();

        slideCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Slide();
    }

    void Slide()
    {
        if (Input.GetButton("Slide") && canSlide)
        {
            if (Mathf.Abs(rb.velocity.x) > 2f && rb.velocity.y < 0.1f && jump.isGrounded)
            {
                capsuleCollider.enabled = false;
                slideCollider.enabled = true;

                isGroundSliding = true;
                // Kayma animasyonunu çalýþtýr
                animator.SetBool("Slide", true);

                if (movement.isFacingRight)
                {
                    rb.AddForce(Vector2.right * 10f, ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(Vector2.right * -1 * 10f, ForceMode2D.Impulse);
                }

                StartCoroutine(StopSliding());
                StartCoroutine(SlideCooldown());
            }
        }
    }

    private IEnumerator StopSliding()
    {
        yield return new WaitForSeconds(0.4f);
        isGroundSliding = false;
        // Kayma animasyonunu durdur
        animator.SetBool("Slide", false);

        capsuleCollider.enabled = true;
        slideCollider.enabled = false;
    }

    private IEnumerator SlideCooldown()
    {
        canSlide = false;

        yield return new WaitForSeconds(1f);

        canSlide = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isGroundSliding && collision.gameObject.CompareTag("Turret"))
        {
            collision.gameObject.GetComponent<Turret>().isExploded = true;
            Destroy(collision.gameObject, 0.5f);
        }
    }
}

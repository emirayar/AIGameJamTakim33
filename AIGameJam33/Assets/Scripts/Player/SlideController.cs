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
    //private Stamina stamina;

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
        //stamina = GetComponent<Stamina>();

        slideCollider.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        /*if (stamina.currentStamina >= 20)
        {
            Slide();
        }
        else
        {
            Debug.Log("Not Enough Stamina");
            stamina.DecreasingEffect();
        }*/

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
                //stamina.UseStamina(20);

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
}
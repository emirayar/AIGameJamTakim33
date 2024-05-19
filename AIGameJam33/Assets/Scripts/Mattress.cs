using UnityEngine;

public class Mattress : MonoBehaviour
{
    private Animator animator;
    [SerializeField] AudioClip clip;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Bu metod, nesne baþka bir nesneye temas ettiðinde çaðrýlýr.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Temas eden nesne bir "karakter" mi kontrol edin.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Matress Trigger'ý çalýþtýr.
            animator.SetTrigger("Mattress");
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}

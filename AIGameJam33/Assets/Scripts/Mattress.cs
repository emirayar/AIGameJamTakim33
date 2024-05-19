using UnityEngine;

public class Mattress : MonoBehaviour
{
    private Animator animator;
    [SerializeField] AudioClip clip;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Bu metod, nesne ba�ka bir nesneye temas etti�inde �a�r�l�r.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Temas eden nesne bir "karakter" mi kontrol edin.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Matress Trigger'� �al��t�r.
            animator.SetTrigger("Mattress");
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}

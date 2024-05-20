using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameOver gameover;
    Animator animator;
    void Start()
    {
        gameover = FindObjectOfType<GameOver>();
        animator = GetComponent<Animator> ();
    }
    void Update()
    {
        animator.SetBool("Shot", true);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarptýðý objenin tag'ini kontrol et
        if (collision.gameObject.tag == "Player")
        {
            gameover.isOver = true;

            // Mermi herhangi bir þeye çarptýðýnda kendini yok et
            Destroy(gameObject);
            animator.SetBool("Shot", false);
            animator.SetTrigger("Hit");
        }

        // Mermi herhangi bir þeye çarptýðýnda kendini yok et
        Destroy(gameObject);
        animator.SetBool("Shot", false);
        animator.SetTrigger("Hit");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; // Ateþlenecek mermi prefabý
    public Transform firePoint; // Merminin çýkýþ noktasý
    public bool isFiring = true;
    public bool isFired;
    [SerializeField] private float fireRate = 3f; // Ateþ etme süresi (saniye)
    [SerializeField] private float fireforce;
    Animator animator;
    public bool isExploded;
    [SerializeField] AudioClip clip;

    private float nextFireTime = 0f;
    void Start()
    {
        animator = GetComponent<Animator> ();
    }
    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            isFired = true;
            nextFireTime = Time.time + fireRate;
        }

        if(isExploded)
        {
            animator.SetTrigger("Explode");
        }
    }

    void Fire()
    {
        if (isFiring)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
            // Mermiyi oluþtur
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Mermiye sola doðru bir hýz ver
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.left * fireforce; // Merminin hýzýný ayarlayabilirsiniz
            isFired = false;
        }
    }
}

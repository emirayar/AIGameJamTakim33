using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab; // Ate�lenecek mermi prefab�
    public Transform firePoint; // Merminin ��k�� noktas�
    public bool isFiring = true;
    public bool isFired;
    [SerializeField] private float fireRate = 3f; // Ate� etme s�resi (saniye)
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
            // Mermiyi olu�tur
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Mermiye sola do�ru bir h�z ver
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.left * fireforce; // Merminin h�z�n� ayarlayabilirsiniz
            isFired = false;
        }
    }
}

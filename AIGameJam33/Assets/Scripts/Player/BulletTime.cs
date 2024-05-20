using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    // Bullet Time kontrol de�i�kenleri
    private bool isBulletTime;
    private float originalTimeScale;
    private float originalFixedDeltaTime;

    private float bulletTimeTimer;
    // Start is called before the first frame update
    void Start()
    {
        originalTimeScale = Time.timeScale;
        originalFixedDeltaTime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Bullet Time'i a�/kapat
            ToggleBulletTime();
        }

        // Bullet Time s�resi dolmu�sa, Bullet Time'� kapat
        if (isBulletTime && Time.time - bulletTimeTimer >= 1f)
        {
            ToggleBulletTime();
        }
    }

    // Bullet Time'i a�ma/kapatma metodu
    void ToggleBulletTime()
    {
        // E�er Bullet Time a��k de�ilse
        if (!isBulletTime)
        {
            // Bullet Time'i a�, zaman �l�e�ini d���r ve Timer'i ba�lat.
            isBulletTime = true;
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
            bulletTimeTimer = Time.time;
        }
        // E�er Bullet Time a��ksa
        else
        {
            // Bullet Time'i kapat, zaman �l�e�ini orijinal de�ere geri getir ve Timer'i s�f�rla.
            isBulletTime = false;
            Time.timeScale = originalTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
    }
}

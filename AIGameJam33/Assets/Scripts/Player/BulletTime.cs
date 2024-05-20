using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    // Bullet Time kontrol deðiþkenleri
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
            // Bullet Time'i aç/kapat
            ToggleBulletTime();
        }

        // Bullet Time süresi dolmuþsa, Bullet Time'ý kapat
        if (isBulletTime && Time.time - bulletTimeTimer >= 1f)
        {
            ToggleBulletTime();
        }
    }

    // Bullet Time'i açma/kapatma metodu
    void ToggleBulletTime()
    {
        // Eðer Bullet Time açýk deðilse
        if (!isBulletTime)
        {
            // Bullet Time'i aç, zaman ölçeðini düþür ve Timer'i baþlat.
            isBulletTime = true;
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
            bulletTimeTimer = Time.time;
        }
        // Eðer Bullet Time açýksa
        else
        {
            // Bullet Time'i kapat, zaman ölçeðini orijinal deðere geri getir ve Timer'i sýfýrla.
            isBulletTime = false;
            Time.timeScale = originalTimeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
    }
}

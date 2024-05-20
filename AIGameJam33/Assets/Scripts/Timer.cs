using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime; // Toplam s�re
    public float timeRemaining; // Kalan s�re
    public TMP_Text timerText; // Timer'�n ekrandaki metni (TextMeshPro kullan�yoruz)
    private bool isTimerRunning = false; // Timer'�n �al���p �al��mad���n� kontrol etmek i�in flag
    public RewindController rewind;
    public GameOver gameover;

    void Start()
    {
        timeRemaining = totalTime; // Ba�lang��ta kalan s�reyi toplam s�reye e�itliyoruz
        UpdateTimerText(); // Timer'� ekrana yazd�r�yoruz
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning && !rewind.isRewinding)
        {
            // E�er timer �al���yorsa, her frame'de kalan s�reyi azalt�yoruz
            timeRemaining -= Time.deltaTime;

            // E�er kalan s�re 0'dan k���kse, timer'� durduruyoruz
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isTimerRunning = false;
                Debug.Log("S�re doldu!");
                gameover.isOver = true;
                StopTimer();
                // ��lem yapmak i�in gerekli kodu buraya ekleyebilirsiniz.
            }

            // Her frame'de timer'� g�ncelliyoruz
            UpdateTimerText();
        }
    }

    public void UpdateTimerText()
    {
        // Metni "dakika:saniye" format�nda g�ncelliyoruz
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Timer metnini g�ncelliyoruz
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        timeRemaining = totalTime;
        UpdateTimerText();
    }
}

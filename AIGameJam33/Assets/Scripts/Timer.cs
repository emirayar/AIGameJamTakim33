using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime; // Toplam süre
    public float timeRemaining; // Kalan süre
    public TMP_Text timerText; // Timer'ýn ekrandaki metni (TextMeshPro kullanýyoruz)
    private bool isTimerRunning = false; // Timer'ýn çalýþýp çalýþmadýðýný kontrol etmek için flag
    public RewindController rewind;
    public GameOver gameover;

    void Start()
    {
        timeRemaining = totalTime; // Baþlangýçta kalan süreyi toplam süreye eþitliyoruz
        UpdateTimerText(); // Timer'ý ekrana yazdýrýyoruz
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning && !rewind.isRewinding)
        {
            // Eðer timer çalýþýyorsa, her frame'de kalan süreyi azaltýyoruz
            timeRemaining -= Time.deltaTime;

            // Eðer kalan süre 0'dan küçükse, timer'ý durduruyoruz
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isTimerRunning = false;
                Debug.Log("Süre doldu!");
                gameover.isOver = true;
                StopTimer();
                // Ýþlem yapmak için gerekli kodu buraya ekleyebilirsiniz.
            }

            // Her frame'de timer'ý güncelliyoruz
            UpdateTimerText();
        }
    }

    public void UpdateTimerText()
    {
        // Metni "dakika:saniye" formatýnda güncelliyoruz
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        // Timer metnini güncelliyoruz
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

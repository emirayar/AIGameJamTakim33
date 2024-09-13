using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // Oyun kapatma fonksiyonu
    public void QuitGame()
    {
        // Unity Editor içindeyken oyunu durdurur
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // Derlenmiş oyun dosyasında oyunu kapatır
        Application.Quit();
        #endif
    }
}

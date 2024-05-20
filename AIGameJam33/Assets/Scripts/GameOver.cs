using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] TextMeshProUGUI[] textMeshPro;
    public bool isOver;
    // Update is called once per frame
    void Update()
    {
        if (isOver)
        {
            foreach (Image img in images)
            {
                img.gameObject.SetActive(true);
            }
            foreach (TextMeshProUGUI text in textMeshPro)
            {
                text.gameObject.SetActive(true);
            }
        }
        if(!isOver)
        {
            foreach (Image img in images)
            {
                img.gameObject.SetActive(false);
            }
            foreach (TextMeshProUGUI text in textMeshPro)
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClapperBoard : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] TextMeshProUGUI[] textMeshPro;
    [SerializeField] AudioClip clip;
    Animator animator;
    GameObject player;
    Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("ClapperBoard");
        StartCoroutine(EnableElements());
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            position = player.GetComponent<Transform>().position;
        }
    }

    IEnumerator EnableElements()
    {
        yield return new WaitForSeconds(0.55f);

        // Enable all images in the array
        foreach (Image img in images)
        {
            img.gameObject.SetActive(true);
        }
        foreach(TextMeshProUGUI text in textMeshPro)
        {
            text.gameObject.SetActive(true);
        }
    }

    public void PlayClip()
    {
        if (player != null)
        {
            position = player.GetComponent<Transform>().position;
            AudioSource.PlayClipAtPoint(clip, position);
        }
    }
}

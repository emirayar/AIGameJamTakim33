using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour
{
    public bool isRewinding = false;
    public float recordTime = 5f;

    List<Vector3> positions;
    List<Quaternion> rotations;
    List<Vector2> velocities;
    List<float> angularVelocities;
    List<AnimatorStateInfo> animationStates;

    Rigidbody2D rb;
    Animator animator;
    public AudioSource audioSource; // AudioSource referansý

    public GameOver gameover;

    public Turret turret;

    // CountdownTimer scriptine eriþmek için CountdownTimer bileþenine referans oluþturuyoruz.
    public CountdownTimer countdownTimer;

    private BackgroundMusicController backgroundMusicController;

    // Use this for initialization
    void Start()
    {
        positions = new List<Vector3>();
        rotations = new List<Quaternion>();
        velocities = new List<Vector2>();
        angularVelocities = new List<float>();
        animationStates = new List<AnimatorStateInfo>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // BackgroundMusicController scriptini bul
        GameObject backgroundMusicObject = GameObject.Find("BackgroundMusic");
        if (backgroundMusicObject != null)
        {
            backgroundMusicController = backgroundMusicObject.GetComponent<BackgroundMusicController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        StopMusic();
        if (Input.GetButtonDown("Rewind"))
            StartRewind();
        if (Input.GetButtonUp("Rewind"))
            StopRewind();
    }

    void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
            if (countdownTimer != null)
            {
                countdownTimer.timeRemaining += Time.fixedDeltaTime; // Countdown timer'ý geri almak için zamaný ayarlýyoruz
                countdownTimer.UpdateTimerText(); // Timer'ý güncelleyerek ekrana yansýtýyoruz
            }
        }
        else
        {
            Record();
        }
    }

    void Rewind()
    {
        if (positions.Count > 0)
        {
            gameover.isOver = false;
            turret.isFiring = false;
            transform.position = positions[positions.Count - 1];
            transform.rotation = rotations[rotations.Count - 1];
            rb.velocity = velocities[velocities.Count - 1];
            rb.angularVelocity = angularVelocities[angularVelocities.Count - 1];
            animator.Play(animationStates[animationStates.Count - 1].fullPathHash, -1, animationStates[animationStates.Count - 1].normalizedTime);

            positions.RemoveAt(positions.Count - 1);
            rotations.RemoveAt(rotations.Count - 1);
            velocities.RemoveAt(velocities.Count - 1);
            angularVelocities.RemoveAt(angularVelocities.Count - 1);
            animationStates.RemoveAt(animationStates.Count - 1);
        }
        else
        {
            StopRewind();
        }
    }

    void Record()
    {
        if (positions.Count > Mathf.Round(recordTime / Time.fixedDeltaTime))
        {
            positions.RemoveAt(0);
            rotations.RemoveAt(0);
            velocities.RemoveAt(0);
            angularVelocities.RemoveAt(0);
            animationStates.RemoveAt(0);
        }

        positions.Add(transform.position);
        rotations.Add(transform.rotation);
        velocities.Add(rb.velocity);
        angularVelocities.Add(rb.angularVelocity);
        animationStates.Add(animator.GetCurrentAnimatorStateInfo(0));
    }

    public void StartRewind()
    {
        isRewinding = true;
        rb.isKinematic = true;
        audioSource.Play(); // AudioSource ile sesi çal
    }

    public void StopRewind()
    {
        isRewinding = false;
        rb.isKinematic = false;
        turret.isFiring = true;
        audioSource.Stop(); // AudioSource ile sesi durdur
    }

    void StopMusic()
    {
        // isRewinding durumunu kontrol et ve müziði durdur veya devam ettir
        if (backgroundMusicController != null)
        {
            if (isRewinding && backgroundMusicController.GetComponent<AudioSource>().isPlaying)
            {
                backgroundMusicController.PauseMusic();
            }
            else if (!isRewinding && !backgroundMusicController.GetComponent<AudioSource>().isPlaying)
            {
                backgroundMusicController.ResumeMusic();
            }
        }
    }
}

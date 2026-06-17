using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [Header("Shake")]
    public float duration = 1f;

    [Header("Intro Explosion")]
    public bool playIntroExplosion = true;

    public float repeatTime = 50f;

    public AudioSource explosionSound;

    Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;

        //lap lai shake sau 1 khoang thoi gian
        if (playIntroExplosion)
        {
            StartCoroutine(ExplosionLoop());
        }
    }
// shake boi cua
    public void TriggerShake()
    {
        StartCoroutine(Shaking());
    }

    
    public void TriggerIntroExplosion()
    {
        if (explosionSound != null)
        {
            explosionSound.Play();
        }

        StartCoroutine(Shaking());
    }

   
    IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.localPosition =
                originalLocalPos +
                Random.insideUnitSphere * 2f;

            yield return null;
        }

        transform.localPosition = originalLocalPos;
    }

    
    IEnumerator ExplosionLoop()
    {
        while (true)
        {
            if (explosionSound != null)
            {
                explosionSound.Play();
            }

            yield return StartCoroutine(Shaking());

            yield return new WaitForSeconds(repeatTime);
        }
    }
}
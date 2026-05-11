using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float duration = 1f;
    public bool start = false;

    public float repeatTime = 15f;

    public float strength = 0.2f;

    public AudioSource explosionSound;

    

    Vector3 originalLocalPos;

    void Start()
    {
        originalLocalPos = transform.localPosition;

        StartCoroutine(ExplosionLoop());
    }

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(Shaking());
        }
    }

    IEnumerator Shaking()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            transform.localPosition =
                originalLocalPos +
                Random.insideUnitSphere;

            yield return null;
        }

        transform.localPosition = originalLocalPos;
    }

    IEnumerator ExplosionLoop()
    {
        while (true)
        {
            // phát tiếng nổ
            if (explosionSound != null)
            {
                explosionSound.Play();
            }

            // rung camera
            yield return StartCoroutine(Shaking());

            // chờ tới lần nổ tiếp
            yield return new WaitForSeconds(repeatTime);
        }
    }
}
using System.Collections;
using UnityEngine;

public class IntroLock : MonoBehaviour
{
    public static bool introFinished = false;

    public MonoBehaviour playerLook;
    public MonoBehaviour playerMove;

    public float introTime = 5f;

    IEnumerator Start()
    {
        introFinished = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerLook != null)
            playerLook.enabled = false;

        if (playerMove != null)
            playerMove.enabled = false;

        yield return new WaitForSeconds(introTime);

        if (playerLook != null)
            playerLook.enabled = true;

        if (playerMove != null)
            playerMove.enabled = true;

        introFinished = true;
    }
}
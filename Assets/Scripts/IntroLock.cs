using System.Collections;
using UnityEngine;

public class IntroLock : MonoBehaviour
{
    public static bool introFinished = false;

    public MonoBehaviour playerLook;
    public MonoBehaviour playerMove;

    public Interaction interaction;
    public float introTime = 4.3f;

    IEnumerator Start()
    {
        introFinished = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerLook != null)
            playerLook.enabled = false;

        if (playerMove != null)
            playerMove.enabled = false;

        if (interaction != null)
            interaction.enabled = false;

        yield return new WaitForSeconds(introTime);

        if (playerLook != null)
            playerLook.enabled = true;

        if (playerMove != null)
            playerMove.enabled = true;

        if (interaction != null)
            interaction.enabled = true;

        introFinished = true;
    }


}
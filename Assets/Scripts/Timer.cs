using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLeft = 180f;

    public TextMeshProUGUI timerText;   
    private PlayerStatus ps;

    bool isRunning = false;

    void Start()
    {
        ps = FindFirstObjectByType<PlayerStatus>();

        UpdateTimerUI();
    }

    void Update()
    {
        // đợi intro kết thúc
        if (!IntroLock.introFinished)
            return;

        isRunning = true;

        //if (!isRunning)
        //    return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0)
        {
            timeLeft = 0;
            isRunning = false;

            Debug.Log("GAME OVER");

            if (ps != null)
            {
                ps.Die();
            }
        }

        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);

        int seconds = Mathf.FloorToInt(timeLeft % 60);

        timerText.text =
            string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
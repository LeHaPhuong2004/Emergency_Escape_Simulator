using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStatus playerStatus;
    public Timer timer;

    [Header("UI")]
    public GameObject winPanel;

    public TextMeshProUGUI scoreText;

    [Header("Stars")]
    public Image star1;
    public Image star2;
    public Image star3;

    [Header("Game Time")]
    public float maxGameTime = 180f;

    float finalScore;

    bool escaped = false;

    private void Start()
    {
        winPanel.SetActive(false);

        SetStarDim(star1);
        SetStarDim(star2);
        SetStarDim(star3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (escaped) return;

        if (other.CompareTag("Player"))
        {
            escaped = true;

            CalculateScore();

            ShowResultUI();
        }
    }

    void CalculateScore()
    {
        float healthScore =
            (playerStatus.currentHealth /
             playerStatus.maxHealth) * 1000f;

        float timeUsed =
            maxGameTime - timer.timeLeft;

        float timeScore =
            (1 - (timeUsed / maxGameTime)) * 1000f;

        timeScore = Mathf.Clamp(timeScore, 0, 1000f);

    
        float breathScore =
            (playerStatus.currentBreath /
             playerStatus.maxBreath) * 500f;

        
        finalScore =
            healthScore +
            timeScore +
            breathScore;
    }

    void ShowResultUI()
    {
        winPanel.SetActive(true);

        scoreText.text =
            "Score: " + Mathf.RoundToInt(finalScore);

        int starCount = 0;

        if (finalScore >= 2200)
            starCount = 3;

        else if (finalScore >= 1700)
            starCount = 2;

        else if (finalScore >= 1200)
            starCount = 1;

        // delay 1 frame để panel active 
        StartCoroutine(ShowStars(starCount));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;
    }
    IEnumerator ShowStars(int starCount)
    {
        yield return null;

        if (starCount >= 1)
            FillStar(star1, 0f);

        if (starCount >= 2)
            FillStar(star2, 0.2f);

        if (starCount >= 3)
            FillStar(star3, 0.4f);
    }

    void FillStar(Image star, float delay)
    {
        if (star == null) return;

        Color c = star.color;
        c.a = 1f;
        star.color = c;

        star.transform.localScale = Vector3.zero;

        star.transform
            .DOScale(1f, 0.4f)
            .SetDelay(delay)
            .SetEase(Ease.OutBack)
            .SetUpdate(true); 
    }

    void SetStarDim(Image star)
    {
        Color c = star.color;
        c.a = 0.08f;
        star.color = c;
    }
}
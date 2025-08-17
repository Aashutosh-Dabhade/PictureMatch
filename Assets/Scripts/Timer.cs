using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public float showAllDuration = 5f;      // time to preview all cards for 5 sec
    public float playDuration = 30f;        // gameplay time
     TMP_Text timerText;              // to show remaining time 
     TMP_Text showAllText;            // show memorize text and preview timer
     Slider timerSlider;              // Slider for timer display
    public CardsController cardsController; 

    private float currentTime;
    private bool isPlaying = false;
    private float gameplayTimeLeft = 0f;

    void Start()
    {
        instance = this;
        SetTimer();
    }

    public void SetTimer()
    { 
        if (UIManager.Instance.showAllText != null)
            UIManager.Instance.showAllText.gameObject.SetActive(true); // Showing memorize text at start
        StartCoroutine(ShowAllCardsRoutine());
    }

    IEnumerator ShowAllCardsRoutine()
    {
        // disabling card interactions while preview
        foreach (Transform card in cardsController.cardParent)
        {
            Card c = card.GetComponent<Card>();
            c.SetInteractable(false);
            c.ShowIcon();
        }

        currentTime = showAllDuration;
        UIManager.Instance.SetShowAllTextActive(true);
        UIManager.Instance.UpdateShowAllUI(currentTime);

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UIManager.Instance.UpdateShowAllUI(currentTime);
        }

        // hiding all cards and enabling card interactions
        foreach (Transform card in cardsController.cardParent)
        {
            Card c = card.GetComponent<Card>();
            c.HideIcon();
            c.SetInteractable(true);
        }

        UIManager.Instance.SetShowAllTextActive(false);

        // starting gameplay timer
        isPlaying = true;
        currentTime = playDuration;
        if (UIManager.Instance.timerSlider != null)
        {
            UIManager.Instance.timerSlider.maxValue = playDuration;
            UIManager.Instance.timerSlider.value = playDuration;
        }
        StartCoroutine(GameplayTimerRoutine());
    }

    IEnumerator GameplayTimerRoutine() //start play duration timer countdown. for eg 30 sec
    {
        gameplayTimeLeft = playDuration;
        UpdateTimerUI();
        while (gameplayTimeLeft > 0)
        {
            yield return null;
            gameplayTimeLeft -= Time.deltaTime;
            UpdateTimerUI();
        }

        isPlaying = false;
        cardsController.GameOverEvent();
    }

    void UpdateTimerUI()
    {
        if (UIManager.Instance.timerText != null)
            UIManager.Instance.timerText.text = Mathf.Ceil(gameplayTimeLeft).ToString();

        if (UIManager.Instance.timerSlider != null)
            UIManager.Instance.timerSlider.value = gameplayTimeLeft;
    }


    public void StopTimer()// stop and reset timer to zero
    {
        StopAllCoroutines();
        isPlaying = false;
        currentTime = 0;
        UpdateTimerUI();
        gameplayTimeLeft = 0;
    }

    void Update()
    {
        // update to smoothen slider value 
        if (isPlaying && UIManager.Instance.timerSlider != null)
        {
            UIManager.Instance.timerSlider.value = gameplayTimeLeft;
        }
    }
}
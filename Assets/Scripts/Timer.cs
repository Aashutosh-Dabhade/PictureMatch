using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public float showAllDuration = 5f;      // time to preview all cards for 5 sec
    public float playDuration = 30f;        // gameplay time
    public TMP_Text timerText;              // to show remaining time 
    public TMP_Text showAllText;            // show memorize text and preview timer
    public Slider timerSlider;              // Slider for timer display
    public CardsController cardsController; 

    private float currentTime;
    private bool isPlaying = false;
    private float gameplayTimeLeft = 0f;
    // Start is called before the first frame update
   void Start()
    {
        instance = this;
        SetTimer();
    }

    public void SetTimer()
    { 
        if (showAllText != null)
            showAllText.gameObject.SetActive(true); // Showing memorize text at start
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
 
       UpdateShowAllUI(currentTime);

        while (currentTime > 0)
        {
            yield return new WaitForSeconds(1f);
            currentTime--;
            UpdateShowAllUI(currentTime);

        }

        // hiding all cards and enabling card interactions
        foreach (Transform card in cardsController.cardParent)
        {
            Card c = card.GetComponent<Card>();
            c.HideIcon();
            c.SetInteractable(true);
        }

       

        // starting gameplay timer
        isPlaying = true;
        currentTime = playDuration;
        if (timerSlider != null)
        {
            timerSlider.maxValue = playDuration;
            timerSlider.value = playDuration;
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

    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = Mathf.Ceil(gameplayTimeLeft).ToString();

        if (timerSlider != null)
            timerSlider.value = gameplayTimeLeft;
    }

 public void UpdateShowAllUI(float showAllTime)// update preview timer
    {
        if (showAllText != null)
            showAllText.text = "Memorize: " + Mathf.Ceil(showAllTime).ToString();
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
        if (isPlaying && timerSlider != null)
        {
            timerSlider.value = gameplayTimeLeft;
        }
    }
}

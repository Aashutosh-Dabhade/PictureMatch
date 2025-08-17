using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }

    [SerializeField] private Difficulty gameDifficulty = Difficulty.Easy;
    [SerializeField] private int easyPairs = 4;
    [SerializeField] private int mediumPairs = 8;
    [SerializeField] private int hardPairs = 12;
    private int pairsToMatch;

    [SerializeField] Card cardPrefab;
    [SerializeField] public Transform cardParent;
    [SerializeField] private Sprite[] sprites;
    private List<Sprite> spritePairs;
    Card firstSelected;
    Card SecondSelected;
    int matchCount;

    void Start()
    {
        PlayerData.Instance.ResetScore(); // reset score to zero
        PlayerData.Instance.LoadHighScore(); // load high score from playerprefs
        UIManager.Instance.UpdateScoreUI(PlayerData.Instance.score, PlayerData.Instance.highScore); 
        PrepareSprites();
        CreateCards();
    }

    public void CreateCards() //create cards and pairs from card prefabs
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card newCard = Instantiate(cardPrefab, cardParent);
            newCard.SelectIconSprite(spritePairs[i]);
            newCard.cardsController = this;
            newCard.gameObject.name = "Card_" + i;
            newCard.ShowIcon();
        }
    }

    public void PrepareSprites()  //based on difficulty level load no of cards pair-from enum
    {
        spritePairs = new List<Sprite>();
        switch (gameDifficulty)
        {
            case Difficulty.Easy:
                pairsToMatch = easyPairs;
                break;
            case Difficulty.Medium:
                pairsToMatch = mediumPairs;
                break;
            case Difficulty.Hard:
                pairsToMatch = hardPairs;
                break;
        }

        for (int i = 0; i < pairsToMatch; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]);
        }

        for (int i = 0; i < spritePairs.Count; i++)
        {
            int randomIndex = Random.Range(i, spritePairs.Count);
            Sprite temp = spritePairs[i];
            spritePairs[i] = spritePairs[randomIndex];
            spritePairs[randomIndex] = temp;
        }
    }

    public void SetSelected(Card card) //select and show clicked icon and check if its matching
    {
        if (card.isSelected)
        {
            card.ShowIcon();
            AudioManager.Instance.PlayShowIcon();
            if (firstSelected == null)
            {
                firstSelected = card;
                return;
            }
            if (SecondSelected == null)
            {
                SecondSelected = card;
                StartCoroutine(CheckMAtching(firstSelected, SecondSelected));
                firstSelected = null;
                SecondSelected = null;
            }
        }
    }

    IEnumerator CheckMAtching(Card a, Card b) //check if first and second selected card is matching
    {
        yield return new WaitForSeconds(0.3f);
        if (a.IconSprite == b.IconSprite)
        {
            matchCount++;
            PlayerData.Instance.AddScore(10);
            UIManager.Instance.UpdateScoreUI(PlayerData.Instance.score, PlayerData.Instance.highScore);

            AudioManager.Instance.PlayCardMatch();
            a.SetInteractable(false);
            b.SetInteractable(false);

            if (matchCount >= spritePairs.Count / 2)
            {
                AudioManager.Instance.PlayWin();
                UIManager.Instance.ShowWinPanel(true);
                Timer.instance.StopTimer();
            }
        }
        else
        {
            a.HideIcon();
            b.HideIcon();
        }
    }

    public void GameOverEvent()
    {
        AudioManager.Instance.PlayGameOver();
        UIManager.Instance.ShowGameOverPanel(true);
        Timer.instance.StopTimer();
    }

    public void RestartGame() // restart game at current level.set slider and timer to zero
    {
        foreach (Transform child in cardParent)
        {
            Destroy(child.gameObject);
        }
        Timer.instance.StopTimer();
        matchCount = 0;
        firstSelected = null;
        SecondSelected = null;
        UIManager.Instance.ShowWinPanel(false);
        UIManager.Instance.ShowGameOverPanel(false);

        PlayerData.Instance.ResetScore();
        UIManager.Instance.UpdateScoreUI(PlayerData.Instance.score, PlayerData.Instance.highScore);

        PrepareSprites();
        CreateCards();
        Timer.instance.SetTimer();
    }

    public void SetDifficulty(Difficulty difficulty) 
    {
        Timer.instance.SetTimer();
        gameDifficulty = difficulty;
    }
    public void SetDifficulty(int difficulty) // for setting difficulty from button
    {
        Timer.instance.SetTimer();
        gameDifficulty = (Difficulty)difficulty;
        RestartGame();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}


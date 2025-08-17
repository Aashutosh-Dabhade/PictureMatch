using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    private List<Sprite> spritePairs;
    [SerializeField] Card cardPrefab;
    [SerializeField] public Transform cardParent;

    Card firstSelected;
    Card SecondSelected;
    int matchCount;
    
    void Start()
    {
        PrepareSprites();
        CreateCards();
    }
    public void PrepareSprites()
    {
        spritePairs = new List<Sprite>();
       

        for (int i = 0; i<sprites.Length; i++)
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

    public void CreateCards() 
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card newCard = Instantiate(cardPrefab, cardParent);
            newCard.SelectIconSprite(spritePairs[i]);
            newCard.gameObject.name = "Card_" + i;
            newCard.cardsController = this;
         //   newCard.ShowIcon();
        }
    }

public void SetSelected(Card card) //select and show clicked icon and check if its matching
    {
        if (card.isSelected)
        {
            card.ShowIcon();
            
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
            a.SetInteractable(false);
            b.SetInteractable(false);

            if (matchCount >= spritePairs.Count / 2)
            {
                Debug.Log("All cards matched!");
            }
            else
            {
                Debug.Log("Matched: " + matchCount);
            }
        }
        else
        {
            a.HideIcon();
            b.HideIcon();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

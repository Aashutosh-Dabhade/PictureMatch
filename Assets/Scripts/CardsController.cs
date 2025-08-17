using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsController : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    private List<Sprite> spritePairs;
    [SerializeField] Card cardPrefab;
    [SerializeField] public Transform cardParent;

    
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
            newCard.ShowIcon();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

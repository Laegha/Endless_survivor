using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeck
{
    const float _cardsPerHand = 5;
    List<PokerCard> _deck;
    public PokerDeck(List<PokerCard> deck)
    {
        _deck = deck;
    }

    public PokerHand DrawHand()
    {
        List<PokerCard> cards = new();
        while(cards.Count < _cardsPerHand) 
        {
            PokerCard randomCard = _deck[Random.Range(0, _deck.Count)];
            cards.Add(randomCard);
            _deck.RemoveAll(x => x.IsSameAs(randomCard));
        }

        return new(cards);
    }
}

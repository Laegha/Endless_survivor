using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerCard 
{
    public enum PokerSuit
    {
        Spades = 0,
        Clubs = 1,
        Diamonds = 2,
        Hearts = 3
    }
    int _cardRank;
    PokerSuit _cardSuit;

    public int CardRank { get { return _cardRank; } }
    public PokerSuit CardSuit { get { return _cardSuit; } }
    public PokerCard(int rank, PokerSuit suit)
    {
        _cardRank = rank;
        _cardSuit = suit;
    }

    public bool IsSameAs(PokerCard otherCard)
    {
        return otherCard.CardSuit == _cardSuit && otherCard.CardRank == _cardRank;
    }
}

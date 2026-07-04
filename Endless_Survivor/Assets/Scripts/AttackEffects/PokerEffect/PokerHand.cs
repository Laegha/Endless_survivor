using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PokerHand
{
    public enum PokerHandPattern
    {
        HighCard = 0,
        Pair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        Straight = 4,
        Flush = 5,
        FullHouse = 6,
        Poker = 7, 
        StraightFlush = 8
    }
    List<PokerCard> _cards;
    public List<PokerCard> Cards { get { return _cards; } }
    public PokerHand(List<PokerCard> cards)
    {
        _cards = cards; 
    }
    public PokerHandPattern GetHandPattern()
    {
        Dictionary<int, int> repeatedCards = new Dictionary<int, int>();
        foreach (var card in _cards)
        {
            if(repeatedCards.ContainsKey(card.CardRank))
            {
                repeatedCards[card.CardRank]++;
                continue;
            }
            repeatedCards.Add(card.CardRank, 1);


        }
        if(IsStraightFlush())
            return PokerHandPattern.StraightFlush;
        
        if (repeatedCards.Values.Any(x => x == 4))
            return PokerHandPattern.Poker;
        
        if (repeatedCards.Values.Any(x => x == 3) && repeatedCards.Values.Any(x => x == 2))
            return PokerHandPattern.FullHouse;
        
        if (IsFlush())
            return PokerHandPattern.Flush;
        
        if (IsStraight())
            return PokerHandPattern.Straight;
        
        if (repeatedCards.Values.Any(x => x == 3))
            return PokerHandPattern.ThreeOfAKind;
        
        if (repeatedCards.Values.Where(x => x == 2).Count() == 2)
            return PokerHandPattern.TwoPair;

        if (repeatedCards.Values.Any(x => x == 2))
            return PokerHandPattern.Pair;

        return PokerHandPattern.HighCard;
    }
    bool IsStraight()
    {
        _cards.Sort((a, b) => a.CardRank.CompareTo(b.CardRank));
        for(int i = 0; i < _cards.Count-1; i++)
        {
            if(_cards[i].CardRank +1 != _cards[i + 1].CardRank)
                return false;
        }
        return true;
    }
    bool IsFlush()
    {
        int sameSuitCards = _cards.Where(x => x.CardSuit == _cards[0].CardSuit).Count();
        
        if(sameSuitCards != _cards.Count) 
            return false;
        return true;
    }
    bool IsStraightFlush() => IsFlush() && IsStraight();
}

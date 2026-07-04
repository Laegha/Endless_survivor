using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PokerSuit = PokerCard.PokerSuit;

public class PokerDeckAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] List<PokerHandEffectsInfo> _effects;
    [SerializeField] List<PokerCardVisuals> _suitsAnimations = new(); 
    [SerializeField] float _distBetweenCards;
    [SerializeField] float _cardsDirectionAngleAmplitude;
    PokerDeck _myDeck;
    static Dictionary<WeaponAttackController, int> _weaponsBadHands = new();
    public PokerDeckAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        if (!_weaponsBadHands.ContainsKey(AffectedAttack.ParentWeapon))
            _weaponsBadHands.Add(AffectedAttack.ParentWeapon, 0);
        var pokerDeckOriginal = original as PokerDeckAttackEffect;
        _effects = new List<PokerHandEffectsInfo>(pokerDeckOriginal._effects);
        foreach(var suitAnimation in pokerDeckOriginal._suitsAnimations)
        {
            _suitsAnimations.Add(new(suitAnimation.suit, new(null, suitAnimation.suitAnimation)));
        }
        _distBetweenCards = pokerDeckOriginal._distBetweenCards;
        _cardsDirectionAngleAmplitude = pokerDeckOriginal._cardsDirectionAngleAmplitude;
        List<PokerCard> deck = GetDeck();
        _myDeck = new(deck);
        
        OnAttack += CreateHand;
    }

    List<PokerCard> GetDeck()
    {
        List<PokerCard> deck = new();
        int duplicateCardsAmmount = (int)Mathf.Floor(_weaponsBadHands[AffectedAttack.ParentWeapon] / 2);
        int duplicateCardsRankThreshold = (int)Mathf.Floor(_weaponsBadHands[AffectedAttack.ParentWeapon] * 1.5f);
        duplicateCardsRankThreshold = Mathf.Clamp(duplicateCardsRankThreshold, 0, 11);

        int reducedSuitsAmmount = (int)Mathf.Floor(_weaponsBadHands[AffectedAttack.ParentWeapon] / 3);
        List<int> increasedSuits = new() { 0, 1, 2, 3 };
        increasedSuits = Utility.ShuffleList(increasedSuits);
        increasedSuits = increasedSuits.GetRange(0, Mathf.Clamp(3 - reducedSuitsAmmount, 1, 5));
        for(int rank = 1; rank <= 13; rank++)
        {
            int cardCopies = rank > duplicateCardsRankThreshold ? duplicateCardsAmmount + 1 : 1;
            cardCopies = Mathf.Clamp(cardCopies, 0, 5);
            for (int i = 0; i < cardCopies; i++)
            {
                for(int suit = 0; suit <= 3; suit++)
                {
                    int suitCopies = increasedSuits.Contains(suit) ? duplicateCardsAmmount + 1 : 1;
                    suitCopies = Mathf.Clamp(suitCopies, 0, 5);
                    for (int j = 0 ; j < suitCopies; j++)
                    {
                        deck.Add(new(rank, (PokerSuit)suit));
                    }
                }
            }
        }
        return deck;
    }

    void CreateHand()
    {
        var hand = _myDeck.DrawHand(); 
        
        //string handDebug = "Hand: ";
        //foreach (var card in hand.Cards)
        //{
        //    handDebug += card.CardRank + " of " + card.CardSuit + ", ";
        //}
        //Debug.Log(handDebug);
        
        var pattern = hand.GetHandPattern();
        var handEffectsInfo = _effects.Find(x => x.TriggeringPattern == pattern);
        List <AttackEffectData> handAttackEffects = null;
        ParticleSystem handPatternParticles = null;
        if (handEffectsInfo != null )
        {
            handAttackEffects = handEffectsInfo.TriggeredEffects;
            handPatternParticles = handEffectsInfo.PatternParticles;

        }
        List<ProyectileAttack> cardAttacks = CreateAttacks();
        

        for (int i = 0; i < 5; i++)
        {
            ProyectileAttack proyectile = cardAttacks[i];
            if(handAttackEffects != null)
                proyectile.EffectsHandler.AddEffects(handAttackEffects);

            //Get sprite with hand.cards[i]
            CustomAnimation cardAnimation = _suitsAnimations.Find(x => x.suit == hand.Cards[i].CardSuit)?.suitAnimation;
            //apply sprite to attack
            if(cardAnimation != null)
            {
                proyectile.Animator.AddAnimations(new() { cardAnimation });
                proyectile.Animator.ChangeAnim(cardAnimation.AnimationName);
            }
        }
        if(handPatternParticles != null)
            ParticleManager.pm.SpawnParticles(new(handPatternParticles, AffectedAttack.ParentWeapon.WeaponControl.transform.position, Quaternion.identity, handPatternParticles.main.duration, null, false, false));
        if (pattern > PokerHand.PokerHandPattern.Straight)
            _weaponsBadHands[AffectedAttack.ParentWeapon] = 0;
        else
            _weaponsBadHands[AffectedAttack.ParentWeapon]++;
    }

    List<ProyectileAttack> CreateAttacks()
    {
        List<ProyectileAttack> createdAttacks = new List<ProyectileAttack>();
        Vector2 splitAttackPos = AffectedAttack.transform.position;
        for (int i = 0; i < 5; i++)
        {
            splitAttackPos = (Vector2)AffectedAttack.transform.position + (Vector2)AffectedAttack.transform.up * _distBetweenCards * i * Mathf.Pow(-1, i + 1);
            float originalRotation = AffectedAttack.transform.rotation.eulerAngles.z;
            float splitAttackRotation = originalRotation + _cardsDirectionAngleAmplitude / 5 * i * Mathf.Pow(-1, i + 1);
            Vector2 splitAttackDir = new Vector2(Mathf.Cos(splitAttackRotation * Mathf.Deg2Rad), Mathf.Sin(splitAttackRotation * Mathf.Deg2Rad));
            Attack createdAttack = null;
            AffectedAttack.ParentWeapon.Attack(splitAttackPos, splitAttackDir, true, out createdAttack);//could actually use the new attack
            createdAttacks.Add((ProyectileAttack)createdAttack);
        }
        GameObject.Destroy(AffectedAttack.gameObject);

        return createdAttacks;
    }

}

[Serializable]
class PokerCardVisuals
{
    public PokerSuit suit;
    public CustomAnimation suitAnimation;

    public PokerCardVisuals(PokerSuit suit, CustomAnimation suitAnimation)
    {
        this.suit = suit;
        this.suitAnimation = suitAnimation;
    }
}
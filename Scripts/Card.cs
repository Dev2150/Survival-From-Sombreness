using System;
using System.Diagnostics;

public class Card
{
    public int tier;
    public string type;
    private float weight;
    private Card[] required;
    private bool taken = false;
    public bool acquired = false;

    public float chosenMaxRange;
    public float chosenRandomNo;

    public Card(string Type, int Tier, float Weight, Card[] Required)
    {
        type = Type;
        tier = Tier;
        weight = Weight;
        required = Required;
    }

    static float sumWeights (Card[] c)
    {
        float s = 0;

        if (c == null || c.Length == 0)
            return 0;

        try
        {
            for (int i = 0; i < c.Length; i++)
                s += c[i].weight;
        }
        catch
        {
            s = 0;
        }
        return s;
    }
    
    private static bool HasMetRequirements (Card card, Card[] deck)
    {
        if (card.type[0] == 'W')
        {

        }

        bool answer = true;
        for (int j = 0; j < card.required.Length; ++j)
                if (!card.required[j].acquired) 
					answer = false;
        return answer;
    }

    public static Card[] accessibleCards(Card[] deck)
    {
        Card[] pool = new Card[deck.Length];
        int cardsNo = 0;
        for (int i = 0; i < deck.Length; i++)
        {
            if (!deck[i].taken && HasMetRequirements(deck[i], deck))
            {
                pool[cardsNo] = deck[i];
                cardsNo++;
            }
        }
        Card[] candidates = new Card[cardsNo];
        for (int i = 0; i < cardsNo; ++i)
        {
            candidates[i] = pool[i];
        }
        return candidates;
    }

    public static Card[] PickFromDeck(Card[] deck, int e)
    {
        Card[] candidates = accessibleCards(deck);

        Card[] answer = new Card[e];
        float remainingSum = sumWeights(candidates);

        for (int j = 0; j < e; ++j)
        {
            float r = UnityEngine.Random.Range(0f, remainingSum);

            answer[j] = new Card("DEF", 1, 50, new Card[0]);
            float s = 0;
            bool doneFinding = true;
            for (int i = 0; doneFinding && i < candidates.Length; ++i)
            {
                if (!candidates[i].taken)
                {
                    s += candidates[i].weight;

                    if (s > r)
                    {
                        doneFinding = false;
                        answer[j] = candidates[i];
                        answer[j].taken = true;
                        answer[j].chosenMaxRange = remainingSum;
                        answer[j].chosenRandomNo = r;
                        remainingSum -= answer[j].weight;
                    }
                }
            }
        }

        for (int i = 0; i < candidates.Length; ++i)
            candidates[i].taken = false;

        return answer;
    }

    internal static Card[] RemoveFromDeck(Card card, Card[] c)
    {
        Card[] cards = new Card[c.Length - 1];
        int newCardsCounter = 0;

        for (int i = 0; i < c.Length; ++i)
        {
            if (c[i] != card)
            {
                cards[newCardsCounter] = c[i];
                newCardsCounter++;
            }
        }

        return cards;
    }
}
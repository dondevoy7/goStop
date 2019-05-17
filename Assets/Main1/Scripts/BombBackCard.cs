using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class BombBackCard : MonoBehaviour
    {
        public Card[] cards;

        private Stack<Card> backCardStack;

        // Start is called before the first frame update
        void Start()
        {
            backCardStack = new Stack<Card>(cards);
        }

        public void Initialized()
        {
            foreach (var item in cards)
            {
                RemoveBackCard(item);
            }
        }

        public List<Card> CreateBackCard()
        {
            List<Card> _cardList = new List<Card>();
            for (int i = 0; i < 2; i++)
            {
                if (backCardStack.Count > 0)
                {
                    Card _card = backCardStack.Pop();
                    _card.gameObject.SetActive(true);
                    _cardList.Add(_card);
                }
            }

            return _cardList.ToList();
        }

        public void RemoveBackCard(Card _card)
        {
            _card.transform.position = new Vector3(0, 0, 0);
            _card.gameObject.SetActive(false);
        }


    }
}


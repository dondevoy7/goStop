using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class Deck : MonoBehaviour
    {
        public Card[] cards;
        public Badak badak;
        public GameState gameState;
        

        private Stack<Card> deckCards;
        private List<Card> badakCardList = new List<Card>();
        private SlotPosition[] players = new SlotPosition[2];

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Initialized()
        {
            deckCards.Clear();
            foreach (var item in cards)
            {
                item.MoveDeckSlot();
            }
        }

        public void  ShuffleCards()
        {
            Card[] _shuffle = cards.OrderBy(x => System.Guid.NewGuid()).ToArray();

            for (int i = 0; i < _shuffle.Length; i++)
            {
                if (i == 0)
                    continue;
                _shuffle[i].transform.position = new Vector3(transform.position.x, transform.position.y, _shuffle[i - 1].transform.position.z - 0.03f);
            }

            deckCards = new Stack<Card>(_shuffle);

        }

        public void StartDistribution(SlotPosition[] _players)
        {
            ShuffleCards();

            System.Array.Copy(_players, players, _players.Length);

            StartCoroutine(Distribution());
            gameState.state = GameState.State.Distribution;
        }

        private IEnumerator Distribution()
        {
            for (int i = 0; i < 2; i++)
            {
                //바닥 카드
                for (int j = 0; j < 4; j++)
                {
                    Card _badakCard = deckCards.Pop();
                    badak.SortingBadakCardList(_badakCard);
                    yield return new WaitForSeconds(0.3f);
                }

                //선 카드
                for (int k = 0; k < 5; k++)
                {
                    Card _card = deckCards.Pop();

                    if (players[gameState.first].state == SlotPosition.State.Me)
                    {
                        players[gameState.first].ReceiveMyCard(_card, Card.SlotSate.Me);

                    }
                    else
                    {
                        players[gameState.first].ReceiveMyCard(_card, Card.SlotSate.You);
                    }

                    yield return new WaitForSeconds(0.3f);
                }

                //후 카드
                {
                    for (int l = 0; l < 5; l++)
                    {
                        Card _card = deckCards.Pop();
                        if (players[gameState.second].state == SlotPosition.State.Me)
                        {
                            players[gameState.second].ReceiveMyCard(_card, Card.SlotSate.Me);

                        }
                        else
                        {
                            players[gameState.second].ReceiveMyCard(_card, Card.SlotSate.You);
                        }
                        yield return new WaitForSeconds(0.3f);
                    }


                }

            }

            gameState.state = GameState.State.GamePlaying;

        }

        public Card PopDeckCard(System.Action _action)
        {
            Card _badakCard = deckCards.Pop();
            badak.SortingBadakCardList(_badakCard, _action);

            return _badakCard;
        }
                
    }
}

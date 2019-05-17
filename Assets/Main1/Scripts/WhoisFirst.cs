using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

namespace Main1
{
    public class WhoisFirst : MonoBehaviour
    {
        public Card[] cards;
        public Badak badak;
        public GameState gameState;
        public UnityEvent startGame;

        private Card firstCard;
        private Card secondCard;
        private int count;


        // Start is called before the first frame update
        void Start()
        {
        }

        public void WhosFirst()
        {
            StartCoroutine(StartMove());
        }

        private IEnumerator StartMove()
        {
            gameState.state = GameState.State.First;

            Card[] _shuffle = cards.OrderBy(x => System.Guid.NewGuid()).ToArray();

            for (int i = 0; i < _shuffle.Length; i++)
            {
                if (i == 0)
                    continue;
                _shuffle[i].transform.position = new Vector3(transform.position.x, transform.position.y, _shuffle[i - 1].transform.position.z - 0.03f);
            }

            Stack<Card> _stack = new Stack<Card>(_shuffle);

            yield return new WaitForSeconds(1.0f);

            firstCard = _stack.Pop();
            firstCard.gameObject.SetActive(true);
            firstCard.MoveBadakSlot(badak.badakPositions[2].position, 2, Card.SlotSate.Badak, MoveEnd);

            yield return new WaitForSeconds(1.0f);

            secondCard = _stack.Pop();
            secondCard.gameObject.SetActive(true);
            secondCard.MoveBadakSlot(badak.badakPositions[6].position, 6, Card.SlotSate.Badak, MoveEnd);

        }

        private void MoveEnd()
        {
            ++count;
            if (count == 2)
            {
                if (firstCard.infomation.month > secondCard.infomation.month)
                {
                    gameState.first = 1;
                    gameState.second = 0;
                }
                else
                {
                    gameState.first = 0;
                    gameState.second = 1;
                }

                StartCoroutine(End());

            }
        }

        private IEnumerator End()
        {
            yield return new WaitForSeconds(2.0f);
            Initialized();
            startGame?.Invoke(); //GameManger.StartGame 호출
        }

        private void Initialized()
        {
            firstCard.transform.position = transform.position;
            firstCard.gameObject.SetActive(false);
            secondCard.transform.position = transform.position;
            secondCard.gameObject.SetActive(false);
        }

    }
}

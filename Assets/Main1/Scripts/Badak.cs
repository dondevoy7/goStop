using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class Badak : MonoBehaviour
    {
        public Transform[] badakPositions;
        public GameObject bombParticlePrefab;
        [HideInInspector]
        public List<Card> badakCardList = new List<Card>();
        private int[] emptyPos; //비어있으면 0, 차있으면 1

        private void Awake()
        {
            emptyPos = new int[badakPositions.Length];
            badakCardList.Clear();
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        public void Initialized()
        {
            badakCardList.Clear();
            for (int i = 0; i < emptyPos.Length; i++)
            {
                emptyPos[i] = 0;
            }
        }

        public List<Card> FindPairCard(Card _card)
        {
            List<Card> _cardList = badakCardList.FindAll(x => x.infomation.month.Equals(_card.infomation.month));

            return _cardList;
        }

        public void SortingBadakCardList(Card _card, System.Action _action = null)
        {
            List<Card> _badakCardList = FindPairCard(_card);

            if (_badakCardList.Count == 0)
            {
                int _num = ChargePosition();
                _card.MoveBadakSlot(badakPositions[_num].position, _num, Card.SlotSate.Badak, _action); 
            }
            else
            {
                int _num = _badakCardList.Last().position;
                Vector3 _pos = new Vector3(badakPositions[_num].position.x + _badakCardList.Count * 0.4f, badakPositions[_num].position.y, badakPositions[_num].position.z);
                _card.MoveBadakSlot(_pos, _badakCardList.Last().position, Card.SlotSate.Badak, _action);
            }

            badakCardList.Add(_card);

        }

        private int ChargePosition()
        {
            for (int i = 0; i < emptyPos.Length; i++)
            {
                if (emptyPos[i] == 0)
                {
                    emptyPos[i] = 1;
                    return i;
                }
            }
            return -1;
        }

        public void EmptyPosition(int i)
        {
            emptyPos[i] = 0;
        }


    }
}

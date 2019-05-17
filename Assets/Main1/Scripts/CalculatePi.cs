using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class CalculatePi : MonoBehaviour
    {
        public HaveCard haveCard;

        private int totalPoint;

        // Start is called before the first frame update
        void Start()
        {
        }

        public void Initialized()
        {
            totalPoint = 0;
        }

        public void CalculatePoint(List<Card> _cardList)
        {
            int _totalPoint = _cardList.Sum(x => x.infomation.point);

            if (_totalPoint >= 10)
            {
                totalPoint = _totalPoint - 9;
                haveCard.PointCalculation(totalPoint);
            }

        }

        public List<Card> TakeAwayCard(int _amount)
        {
            int _temp = _amount;
            List<Card> _cardList = new List<Card>();
            for (int i = 0; i < _amount; i++)
            {
                Card _card = haveCard.haveCardList.Find(x => x.infomation.point == 2);
                if (_card != null && _temp >= 2)
                {
                    _amount -= 1;
                    _temp -= 2;
                    _cardList.Add(_card);
                    haveCard.haveCardList.Remove(_card);
                }
                else
                {
                    _card = haveCard.haveCardList.Find(x => x.infomation.point == 1);
                    if (_card != null)
                    {
                        _cardList.Add(_card);
                        haveCard.haveCardList.Remove(_card);
                    }
                }

            }

            SortCardList();

            return _cardList.ToList();
        }

        public void ReceiveCard(Card _card)
        {
            haveCard.CardAdd(_card);
        }

        private void SortCardList()
        {
            int _amout = 0;
            foreach (var item in haveCard.haveCardList)
            {
                Vector3 _pos = new Vector3(haveCard.haveCardPosition.position.x + _amout * HaveCardSlotManager.SPACE,
                    haveCard.haveCardPosition.position.y, haveCard.haveCardPosition.position.z);

                item.transform.position = _pos;
                _amout++;
            }

        }
    }

}

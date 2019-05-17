using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Main1
{

    public class CalculateGwang : MonoBehaviour
    {
        public HaveCard haveCard;
        public ImageText imageText;

        private Vector2 pos = new Vector2();
        private int totalPoint;

        private void OnDestroy()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
                pos = new Vector2(haveCard.haveCardPosition.position.x + 0.4f, haveCard.haveCardPosition.position.y);
        }

        public void Initialized()
        {
            totalPoint = 0;
        }

        public void CalculatePoint(List<Card> _cardList)
        {
            if (_cardList.Count == 3)
            {
                foreach (var item in _cardList)
                {
                    totalPoint = 3;
                    if (item.infomation.subKind.Equals(Infomation.SubKind.Bigwang))
                    {
                        totalPoint = 2;
                        break;
                    }
                }
            }
            else if (_cardList.Count == 4)
            {
                totalPoint = 4;
            }
            else if (_cardList.Count == 5)
            {
                totalPoint = 15;
                imageText.DisplayImageText((int)ImageText.ImageName.Fivegwang, pos);
            }

            haveCard.PointCalculation(totalPoint);

        }

    }

}

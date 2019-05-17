using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class CalculateYulGut : MonoBehaviour
    {
        public HaveCard haveCard;
        public ImageText imageText;

        private int totalPoint;
        private int godoriCount;
        private Vector2 pos = new Vector2();

        private void OnDisable()
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
            godoriCount = 0;
        }

        public void CalculatePoint(List<Card> _cardList)
        {
            if (_cardList.Last().infomation.subKind.Equals(Infomation.SubKind.Godori))
            {
                godoriCount += 1;
                if (godoriCount == 3) //고도리
                {
                    totalPoint += 5;
                    imageText.DisplayImageText((int)ImageText.ImageName.Godori, pos);
                }
            }

            if (_cardList.Count >= 5)
            {
                totalPoint += 1;
            }

            if (_cardList.Count == 7)
            {
                //멍텅구리 x2
            }

            haveCard.PointCalculation(totalPoint);
        }


    }
}


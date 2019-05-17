using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class CalculateDDi : MonoBehaviour
    {
        public HaveCard haveCard;
        public ImageText imageText;

        private int totalPoint;
        private int hongdanCount;
        private int ChyeongdanCount;
        private int ChodanCount;
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
            hongdanCount = 0;
            ChyeongdanCount = 0;
            ChodanCount = 0;
        }

        public void CalculatePoint(List<Card> _cardList)
        {
            if (_cardList.Count >= 5)
            {
                totalPoint += 1;
            }

            switch (_cardList.Last().infomation.subKind)
            {
                case Infomation.SubKind.HongDan:
                    hongdanCount += 1;
                    if (hongdanCount == 3)
                    {
                        totalPoint += 3;
                        imageText.DisplayImageText((int)ImageText.ImageName.Hongdan, pos);

                    }
                    break;
                case Infomation.SubKind.ChyeongDan:
                    ChyeongdanCount += 1;
                    if (ChyeongdanCount == 3)
                    {
                        totalPoint += 3;
                        imageText.DisplayImageText((int)ImageText.ImageName.Chyeondan, pos);
                    }
                    break;
                case Infomation.SubKind.ChoDan:
                    ChodanCount += 1;
                    if (ChodanCount == 3)
                    {
                        totalPoint += 3;
                        imageText.DisplayImageText((int)ImageText.ImageName.Chodan, pos);
                    }
                    break;
                default:
                    break;
            }

            haveCard.PointCalculation(totalPoint);

        }
    }

}

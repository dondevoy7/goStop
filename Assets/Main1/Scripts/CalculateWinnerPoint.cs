using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

namespace Main1
{
    public class CalculateWinnerPoint : MonoBehaviour
    {
        public GameObject pointPanel;
        public GameState gameState;
        public HaveCardSlotManager[] haveCardSlotManagers;
        public TextMeshProUGUI[] texts;

        private int goCount;
        private int totalPoint;

        // Start is called before the first frame update
        void Start()
        {

        }

        public void CalculatePoint()
        {
            pointPanel.SetActive(true);

            goCount = haveCardSlotManagers[gameState.winner].calculateTotalPoint.goCount;
            totalPoint = haveCardSlotManagers[gameState.winner].totalPoint;

            totalPoint += goCount;


            for (int i = 3; i < goCount; i++)
            {
                haveCardSlotManagers[gameState.winner].calculateTotalPoint.square *= 2;
            }


            CalculatePiBak();
            CalculateGwangBak();
            CalculateMeongBak();
            CalculateGoBak();

            int _finalSquare = haveCardSlotManagers[gameState.winner].calculateTotalPoint.square;
            int _originPoint = totalPoint; //배율을 곱하기 전

            totalPoint *= _finalSquare;

            texts[0].SetText(string.Format("{0:N0}", totalPoint * 100));
            texts[1].SetText("{0} X {1}", _originPoint, _finalSquare);
        }

        private void CalculatePiBak()
        {
            int _winner = haveCardSlotManagers[gameState.winner].haveCards[3].totalPoint;

            //진쪽에 9열끗 카드가 있으면 피박 유무를 판단해서 옮긴다
            haveCardSlotManagers[gameState.loser].calculateTotalPoint.isMoveYulgut?.Invoke();
            List<Card> _piList = haveCardSlotManagers[gameState.loser].haveCards[3].haveCardList;
            int _loser = _piList.Sum(x => x.infomation.point);

            if (_winner >= 1)
            {
                if (_loser >= 1 && _loser <= 7)
                {
                    haveCardSlotManagers[gameState.winner].calculateTotalPoint.square *= 2;
                }
            }

        }

        private void CalculateGwangBak()
        {
            int _winner = haveCardSlotManagers[gameState.winner].haveCards[0].totalPoint;
            int _loser = haveCardSlotManagers[gameState.loser].haveCards[0].haveCardList.Count;

            if (_winner >= 3)
            {
                if (_loser <= 0)
                {
                    haveCardSlotManagers[gameState.winner].calculateTotalPoint.square *= 2;
                }
            }
        }

        private void CalculateMeongBak()
        {
            int _winner = haveCardSlotManagers[gameState.winner].haveCards[1].haveCardList.Count;

            if (_winner >= 7)
            {
                haveCardSlotManagers[gameState.winner].calculateTotalPoint.square *= 2;
            }
        }

        private void CalculateGoBak()
        {
            int _goCount = haveCardSlotManagers[gameState.loser].calculateTotalPoint.goCount;
            if (_goCount > 0)
            {
                haveCardSlotManagers[gameState.winner].calculateTotalPoint.square *= 2;
            }
        }
    }

}

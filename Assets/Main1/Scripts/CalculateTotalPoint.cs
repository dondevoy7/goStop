using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Main1
{
    public class CalculateTotalPoint : MonoBehaviour
    {
        public GameObject goStopPanel;
        public GameState gameState;
        public TextMeshProUGUI goText;
        [HideInInspector]
        public int goCount;
        [HideInInspector]
        public int square;
        public System.Action isMoveYulgut;  //9열긋이 있으면 옮길꺼냐

        private Button[] buttons; //go, stop
        private int preTotalPoint; //고를 판단 하기 위해선 전 점수 보다 커야 한다
        private int totalPoint; //HaveCardSlotManager에서 받아서 열긋카드를 옮기는데 사용한다

        // Start is called before the first frame update
        void Start()
        {
            square = 1;
            buttons = goStopPanel.GetComponentsInChildren<Button>();
            buttons[0].onClick.AddListener(PushGoBtn);
            buttons[1].onClick.AddListener(PushStopBtn);

        }

        public void initialized()
        {
            preTotalPoint = 0;
            goCount = 0;
            square = 1;
            totalPoint = 0;
        }

        private bool GoStopDecide(int _totalPoint)
        {
            if (_totalPoint >= 7)
            {
                if (preTotalPoint == 0 || _totalPoint > preTotalPoint)
                {
                    preTotalPoint = _totalPoint;
                    return true;
                }
            }

            return false;
        }

        public IEnumerator SelectGoStop(int _totalPoint)
        {
            totalPoint = _totalPoint;

            if (GoStopDecide(totalPoint) == true)
            {
                if (gameState.cardCount > 0)
                {
                    if (gameState.auto == true)
                    {
                        buttons[1].onClick.Invoke();
                    }
                    else
                    {
                        ActiveGoStopPanel(true);
                    }
                }
                else
                {
                    gameState.state = GameState.State.End;
                }
            }

            yield return new WaitWhile(() => goStopPanel.activeSelf);

            yield return new WaitWhile(()=> goText.gameObject.activeSelf);
        }

        private void PushGoBtn()
        {
            if (totalPoint < 7)
            {
                return;
            }

            goCount++;
            StartCoroutine(DisplayGoCount());
            ActiveGoStopPanel(false);
        }

        private void PushStopBtn()
        {
            if (totalPoint < 7)
            {
                isMoveYulgut?.Invoke();
                return;
            }

            gameState.state = GameState.State.End;
            ActiveGoStopPanel(false);
        }

        private void ActiveGoStopPanel(bool _active)
        {
            goStopPanel.SetActive(_active);

        }

        private IEnumerator DisplayGoCount()
        {
            goText.gameObject.SetActive(true);
            goText.SetText("{0} GO", goCount);
            yield return new WaitForSeconds(2.0f);
            goText.gameObject.SetActive(false);
        }

    }
}

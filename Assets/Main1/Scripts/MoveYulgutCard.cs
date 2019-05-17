using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Main1
{
    public class MoveYulgutCard : MonoBehaviour
    {
        public HaveCard piCard;
        public GameObject selectPanel;
        public GameState gamestate;
        [HideInInspector]
        public int count; //카드 이동 완료 동기화

        private bool active; //한번만 물어보자
        private HaveCard yulgutCard; //
        private Button[] buttons;
        private Card num9Card;
        // Start is called before the first frame update
        void Start()
        {
            yulgutCard = GetComponent<HaveCard>();
            buttons = selectPanel.GetComponentsInChildren<Button>();
            active = false;
        }

        public void Initialized()
        {
            active = false;
        }

        public void CheckMoveYulgutCard()
        {
            if (active == true)
                return;

            if (piCard.haveCardList.Sum(x=>x.infomation.point) > 7)
                return;

            num9Card = yulgutCard.haveCardList.Find(x => x.infomation.subKind.Equals(Infomation.SubKind.BatCard));
            if (num9Card != null)
            {
                active = true;
                piCard.CardAdd(num9Card);
                yulgutCard.haveCardList.Remove(num9Card);
                SortCardList();
            }

        }

        public void MoveCard()
        {
            if (active == true)
                return;

            num9Card = yulgutCard.haveCardList.Find(x => x.infomation.subKind.Equals(Infomation.SubKind.BatCard));
            if (num9Card != null)
            {
                active = true;
                if (gamestate.auto == true)
                {
                    PushOkBtn();
                }
                else
                {
                    ShowSelectPanel(true);
                }
            }
        }

        private void ShowSelectPanel(bool _active)
        {
            if (_active == true)
            {
                buttons[0].onClick.AddListener(PushOkBtn);
                buttons[1].onClick.AddListener(PushCancelBtn);
            }
            else
            {
                buttons[0].onClick.RemoveListener(PushOkBtn);
                buttons[1].onClick.RemoveListener(PushCancelBtn);

            }

            selectPanel.SetActive(_active);

        }

        public void PushOkBtn()
        {
            piCard.CardAdd(num9Card);
            yulgutCard.haveCardList.Remove(num9Card);
            SortCardList();
            count++;
            ShowSelectPanel(false);
        }

        public void PushCancelBtn()
        {
            ShowSelectPanel(false);
        }

        private void SortCardList()
        {
            int _amout = 0;
            foreach (var item in yulgutCard.haveCardList)
            {
                Vector3 _pos = new Vector3(yulgutCard.haveCardPosition.position.x + _amout * HaveCardSlotManager.SPACE,
                    yulgutCard.haveCardPosition.position.y, yulgutCard.haveCardPosition.position.z);

                item.transform.position = _pos;
                _amout++;
            }

        }
    }

}

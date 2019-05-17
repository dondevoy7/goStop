using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using UnityEngine.Events;

namespace Main1
{
    [System.Serializable]
    public class PointEvent : UnityEvent<List<Card>> { }

    public class HaveCard : MonoBehaviour
    {
        public Transform haveCardPosition;
        public GameObject pointObj;
        public PointEvent pointEvent;
        public UnityEvent moveEnd; //각각의 카드 슬롯으로 이동이 끝났는지 확인
        public UnityEvent init; //초기화
        [HideInInspector]
        public int totalPoint;
        [HideInInspector]
        public List<Card> haveCardList = new List<Card>();

        private TextMeshProUGUI pointText;

       
        // Start is called before the first frame update
        void Start()
        {
            pointText = pointObj.GetComponentInChildren<TextMeshProUGUI>();
        }

        public void CardAdd(Card _card)
        {
            Vector3 _pos = new Vector3(haveCardPosition.position.x + haveCardList.Count * HaveCardSlotManager.SPACE, haveCardPosition.position.y, haveCardPosition.position.z);
            _card.MoveHaveSlot(_pos, MoveEnd);
            ChangeCardLayer(_card, "HaveCard");
            haveCardList.Add(_card);
        }

        private void ChangeCardLayer(Card _card, string _name)
        {
            if (_card == null)
                return;

            _card.gameObject.layer = LayerMask.NameToLayer(_name);
            for (int i = 0; i < _card.transform.childCount; i++)
            {
                _card.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer(_name);
            }
        }

        private void MoveEnd(Card _card)
        {
            _card.InitActionCard();
            pointEvent?.Invoke(haveCardList);
            moveEnd?.Invoke();

        }

        public void PointCalculation(int _totalPoint)
        {
            totalPoint = _totalPoint;

            if (pointObj.activeSelf == false && totalPoint > 0)
                pointObj.SetActive(true);

            if (pointObj.activeSelf == true)
            {
                //점수 표시 위치
                pointObj.transform.position = new Vector3(haveCardList.Last().transform.position.x + HaveCardSlotManager.SPACE,
                                                                             pointObj.transform.position.y, pointObj.transform.position.z);
                pointText.SetText("{0}", totalPoint);

            }



        }

        public void InitHaveCard()
        {
            foreach (var item in haveCardList)
            {
                ChangeCardLayer(item, "Default");
            }

            haveCardList.Clear();

            if (pointObj != null)
                pointObj.SetActive(false);

            pointText.SetText("{0}", 0);

            totalPoint = 0;
            init?.Invoke();
        }

    }

}

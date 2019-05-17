using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class SlotPosition : MonoBehaviour
    {
        public enum State { Me, You, None}
        public State state = State.Me;

        public GameState gameState;
        [HideInInspector]
        public List<Card> myCardList;
        public Transform[] myCardPositions;
        public SpriteRenderer[] spritePairs;
        public System.Action<Card> sendCard;

        public Badak badak;
        public BombBackCard bombBackCard;
        public static System.Action moveEndAction;
        public HaveCardSlotManager haveCardSlotManager;
        [HideInInspector]
        public int myTurn;

        private int count; //초가화 필요
        static private int receiveCount; //초기화 필요
        private Bomb bomb;
        private Sprite oringinSprite;

        // Start is called before the first frame update
        void Start()
        {
            moveEndAction += SortAndPairCardList;
            bomb = GameObject.Find("GameManager").GetComponent<Bomb>();

            if (spritePairs.Length > 0)
            {
                oringinSprite = spritePairs[0].sprite;
            }
        }

        public void initialized()
        {
            myCardList.Clear();
            bombBackCard.Initialized();
            haveCardSlotManager.Initialized();
            foreach (var item in spritePairs)
            {
                item.sprite = oringinSprite;
                item.gameObject.SetActive(false);
            }
        }

        //카드 정렬 및 자리 바꿈
        public void SortMyCardList()
        {
            myCardList.Sort((x, y) => x.infomation.number > y.infomation.number ? 1 : -1);

            for (int i = 0; i < myCardList.Count; i++)
            {
                myCardList[i].transform.position = myCardPositions[i].position;
                myCardList[i].slotPosition = i;
            }

        }

        //내 카드 받음
        public void ReceiveMyCard(Card _card, Card.SlotSate _slotState)
        {
            _card.MoveMyOrYourSlot(myCardPositions[count++].position, _slotState, ReceiveActionMoveEndCard);
            myCardList.Add(_card);
        }

        //내 카드를 다 받으면 정렬 및 짝 맞는 카드 표시
        public  void ReceiveActionMoveEndCard()
        {
            receiveCount++;
            if (myCardPositions.Length * 2 == receiveCount)
            {
                moveEndAction?.Invoke();

            }
        }

        public void SortAndPairCardList()
        {
            receiveCount = 0;
            count = 0;

            SortMyCardList();

            if (state == State.Me)
            {
                FindPairBadakCard();
                bomb.BombCardDisplay(spritePairs, myCardList); 
            }
        }

        //짝 맞는 카드 표시 
        public void FindPairBadakCard()
        {
            if (state == State.Me)
            {
                foreach (var item in spritePairs)
                {
                    item.sprite = oringinSprite;
                    item.gameObject.SetActive(false);
                }
            }

            for (int j = 0; j < myCardList.Count; j++)
            {
                Card _pairCard = badak.badakCardList.Find(x => x.infomation.month.Equals(myCardList[j].infomation.month));
                if (_pairCard != null)
                {
                    spritePairs[j].gameObject.SetActive(true);
                }
            }
        }

  

        //내가 낸 카드 
        public void PushSelectMyCardBtn(int _num)
        {
            if (state != State.Me)
                return;

            if (myCardList.Count <= _num)
                return;

            if (gameState.state.Equals((GameState.State)myTurn))
            {
                sendCard?.Invoke(myCardList[_num]);
                gameState.state = GameState.State.GamePlaying;
            }

        }

        //ai
        public void YourTurn()
        {
            if (state != State.You)
                return;

            if (gameState.state.Equals((GameState.State)myTurn))
            {
                int _num = Random.Range(0, myCardList.Count);
                sendCard?.Invoke(myCardList[_num]);
                gameState.state = GameState.State.GamePlaying;
            }
        }

        public void SortMyCard()
        {
            SortMyCardList();

        }

        //낸 카드 판정
        public void PlayerTurnEnd()
        {
            haveCardSlotManager.ReceiveHaveCardListDecide();
        }

        public void CheckFindPairAndBombCard()
        {
            //2. 호출 순서
            if (state == State.Me)
            {
                FindPairBadakCard();
                bomb.BombCardDisplay(spritePairs, myCardList);
            }
        }

        public bool CheckChongTong()
        {
            for (int i = 1; i < 13; i++)
            {
                var _count = myCardList.Count(x => x.infomation.month.Equals(i));
                if (_count == 4)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class HaveCardSlotManager : MonoBehaviour
    {
        public HaveCard[] haveCards;
        public SelectCard selectCard;
        public ImageText imageText;
        public GameState gameState;
        public CalculateTotalPoint calculateTotalPoint;
        public TakePiCard takePiCard;
        public MoveYulgutCard moveYulgutCard;
        [HideInInspector]
        public int totalPoint; //havecard의 총점수 합
        [HideInInspector]
        public int mutiCount; //몇 배수 인가?
        [HideInInspector]
        public int takeCardCount; //몇 장의 카드를 빼았는가?

        public const float SPACE = 0.2f;

        private List<Card> myCardList = new List<Card>();
        private List<Card> deckCardList = new List<Card>();
        private List<Card> tempCardList = new List<Card>();
        private List<Card> selfFukCardList = new List<Card>(); //자뻑 유무
        private Badak badak;
        private bool isSameDeck = false;
        private int moveCount;//havecard 의 움직임이 끝났는가?

        // Start is called before the first frame update
        void Start()
        {
            //9열긋을 옮길거냐?
            calculateTotalPoint.isMoveYulgut = moveYulgutCard.CheckMoveYulgutCard;
        }

        public void Initialized()
        {
            foreach (var item in haveCards)
            {
                item.InitHaveCard();
            }
            calculateTotalPoint.initialized();
            selfFukCardList.Clear();
            moveYulgutCard.Initialized();
            moveCount = 0;
        }

        //내가 낸 카드와 짝이 맞는 것
        public void ReceiveMyCard(Card _card, Badak _badak)
        {
            myCardList = _badak.FindPairCard(_card).ToList();
        }
        //덱에서 뽑은 카드와 짝이 맞는 것
        public void ReceiveDeckCard(Card _card, Badak _badak)
        {
            deckCardList = _badak.FindPairCard(_card).ToList();
            badak = _badak;
        }

        public void ReceiveHaveCards(Card _card, Badak _badak, System.Action<Card, Badak> _action)
        {
            _action?.Invoke(_card, _badak);
        }
        //짝 맞는 카드 판정
        public void ReceiveHaveCardListDecide()
        {
            StartCoroutine(StartCardListDecide());
        }

        private IEnumerator StartCardListDecide()
        {
            //내가 낸 카드 먼저 판정 하고
            yield return StartCoroutine(CardListDecide(myCardList));
            //덱에서 낸 카드 판정한다
            yield return StartCoroutine(CardListDecide(deckCardList));
            //뺏을 카드가 있는가?
            yield return StartCoroutine(TakeCardMove());
            //9열긋을 옮기면 고인가 아니면 피박을 면하는가?
            yield return StartCoroutine(IsMove9Yulgut());
            //go, stop decide
            yield return StartCoroutine(GoStopDecide());


            if (gameState.state.Equals(GameState.State.End) == false)
            {
                gameState.state = GameState.State.TurnEnd;
            }

            InitCardList();
        }

        //초기화
        private void InitCardList()
        {
            myCardList.Clear();
            deckCardList.Clear();
            isSameDeck = false; //초기하는 여기서

        }

        private IEnumerator CardListDecide(List<Card>_cardList)
        {
            tempCardList.Clear();
            int _imageNum = 0;

            //뻑, 따닥 구별:  덱 카드에서만 판정한다
            if (myCardList.Count > 0 && myCardList[0].Equals(deckCardList[0]))
            {
                myCardList.Clear(); //deckCardList와 같아서 삭제함
                isSameDeck = true;
                yield break;
            }

            if (_cardList.Count == 4)
            {
                if (isSameDeck == true)
                {
                    //따닥
                    _imageNum = (int)ImageText.ImageName.Dadak;
                    takeCardCount++; //상대방 피를 가져온다
                }
                else
                {
                    Card _selfFuk = selfFukCardList.Find(x => x.Equals(_cardList[0]));
                    if (_selfFuk != null)
                    {
                        //자뻑
                        takeCardCount += 2;
                        selfFukCardList.Remove(_selfFuk);
                    }
                    else
                    {
                        //자뻑 아님
                        takeCardCount++; //상대방 피를 가져온다
                    }
                }

                //마지막에 뻑이면 덱카드는 가져가지 않는다
                if (gameState.cardCount >= 1)
                {
                    tempCardList = _cardList.ToList();
                }
                else
                {
                    tempCardList.Add(_cardList[0]);
                    tempCardList.Add(_cardList[1]);
                    tempCardList.Add(_cardList[2]);
                }
            }
            else if (_cardList.Count == 3)
            {
                if (isSameDeck == true)
                {
                    //뻑
                    _imageNum = (int)ImageText.ImageName.Fuk;
                    selfFukCardList.Add(_cardList[0]); //자뻑 유무를 가리자
                }
                else
                {
                    //선택
                    yield return StartCoroutine(SelectCard(_cardList.ToList()));
                }
            }
            else if (_cardList.Count == 2)
            {
                if (isSameDeck == true)
                {
                    //쪽
                    _imageNum = (int)ImageText.ImageName.Chyok;
                    takeCardCount++; //상대방 피 뺏기
                }

                tempCardList = _cardList.ToList();
            }

            //맨 마지막은 하지 않는다
            if (isSameDeck == true && gameState.cardCount >= 1)
            {
                Vector3 _pos = new Vector3(_cardList[0].transform.position.x + 0.3f, _cardList[0].transform.position.y, _cardList[0].transform.position.z);
                imageText.DisplayImageText(_imageNum, _pos);
            }

            //쪽,따닥,뻑 이미지 사라질 때 까지...
            yield return new WaitWhile(() => imageText.imageTexts[_imageNum].activeSelf);

            //카드 종류 별로 자리 이동
            foreach (var item in tempCardList)
            {
                haveCards[(int)item.infomation.kind - 1].CardAdd(item);
            }

            //카드의 움직임이 끝날 때 까지 기다림
            yield return new WaitUntil(()=>moveCount.Equals(tempCardList.Count));

            //바닥 카드 제거
            foreach (var item in tempCardList)
            {
                badak.badakCardList.Remove(item);
            }

            //싹쓸
            if (gameState.cardCount >= 1)
            {
                if (badak.badakCardList.Count == 0)
                {
                    takeCardCount++;
                }
            }


            //바닥 빈자리를 다시 만든다
            if (_cardList.Count > 0 && _cardList.Count.Equals(tempCardList.Count))
            {
                badak.EmptyPosition(tempCardList[0].position);
            }

            moveCount = 0; //카드 움직임 초기화

        }

        private IEnumerator SelectCard(List<Card> _cardList)
        {
            List<Sprite> _spriteList = new List<Sprite>();
            
            //카드 종류가 달라야 하고 쌍피여도 안된다
            if ((_cardList[0].infomation.kind.Equals(_cardList[1].infomation.kind) == false) || 
                (_cardList[0].infomation.point.Equals(_cardList[1].infomation.point) == false ) )
            {
                _spriteList.Add(_cardList[0].spriteRenderer[0].sprite);
                _spriteList.Add(_cardList[1].spriteRenderer[0].sprite);

                //선택 창 화면
                if (gameState.auto == true)
                {
                    AutoSelectCard(_cardList);
                }
                else
                {
                    selectCard.ShowSelectCard(_spriteList);
                }

                yield return new WaitWhile(() => selectCard.waitCount != -1);

                tempCardList.Add(_cardList[selectCard.selectNum]);
                tempCardList.Add(_cardList[2]);

            }
            else
            {
                tempCardList.Add(_cardList[1]);
                tempCardList.Add(_cardList[2]);
            }


        }

        //상대방이 ai일 때 자동 선택
        private void AutoSelectCard(List<Card> _cardList)
        {
            selectCard.selectNum = Random.Range(0, 2);
            selectCard.waitCount = -1;
        }

        private IEnumerator GoStopDecide()
        {
            totalPoint = haveCards.Sum(x => x.totalPoint);
            //print(transform.name +" totalPoint: " +  totalPoint);
            yield return StartCoroutine(calculateTotalPoint.SelectGoStop(totalPoint));
        
        }

        private IEnumerator TakeCardMove()
        {
            if (takeCardCount > 0 && gameState.cardCount >= 1)
            {
                takePiCard.TakePi(takeCardCount);
            }
            yield return new WaitUntil(() => moveCount.Equals(takePiCard.count));

            takeCardCount = 0;
            moveCount = 0;
            takePiCard.count = 0;
        }

        private IEnumerator IsMove9Yulgut()
        {
            if (totalPoint >= 5)
            {
                moveYulgutCard.MoveCard();
            }

            yield return new WaitWhile(() => moveYulgutCard.selectPanel.activeSelf);

            yield return new WaitUntil(() => moveCount.Equals(moveYulgutCard.count));

            moveCount = 0;
            moveYulgutCard.count = 0;
        }

        public void MoveEndCount()
        {
            ++moveCount;
        }
    }
}

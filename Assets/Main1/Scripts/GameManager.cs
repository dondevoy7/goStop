using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class GameManager : MonoBehaviour
    {
        public GameState gameState;
        public Deck deck;
        public Bomb bomb;
        public Badak badak;
        public WhoisFirst whoisFirst;
        public CalculateWinnerPoint calculateWinnerPoint;
        public SlotPosition[] players;


        private void Awake()
        {
            Application.targetFrameRate = 30;
#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }
        void Start()
        {

            players[0].sendCard = ReceiveCard;
            players[1].sendCard = ReceiveCard;


            whoisFirst.WhosFirst();
        }

        //WhoisFirst.startGame에 의해서 호출 됨
        public void StartGame()
        {
            players[gameState.first].myTurn = (int)GameState.State.FirstTurn;
            players[gameState.second].myTurn = (int)GameState.State.SecondTurn;

            StartCoroutine("GameProgress");
        }

        private IEnumerator  GameProgress()
        {
            yield return StartCoroutine(GameStart());

            yield return StartCoroutine(ChongTong());

            yield return StartCoroutine("GamePlaying");

            yield return StartCoroutine(GameEnd());
        }

        private IEnumerator GameStart()
        {
            yield return new WaitForSeconds(1.0f);

            deck.StartDistribution(players);


        }

        private IEnumerator ChongTong()
        {
            yield return new WaitWhile(() => gameState.state.Equals(GameState.State.Distribution));

            if (players[gameState.first].CheckChongTong() == true)
            {
                players[gameState.first].haveCardSlotManager.totalPoint = 7;
                gameState.state = GameState.State.End;
            }
            if (players[gameState.second].CheckChongTong() == true)
            {
                players[gameState.second].haveCardSlotManager.totalPoint = 7;
                gameState.state = GameState.State.End;
            }

            yield return null;
        }

        private IEnumerator GamePlaying()
        {

            while (gameState.state.Equals(GameState.State.End) == false)
            {
                gameState.state = GameState.State.FirstTurn;


                if (players[gameState.first].state== SlotPosition.State.You)
                {
                    players[gameState.first].YourTurn();
                    gameState.auto = true;
                }
                else
                {
                    gameState.auto = false;
                }

                gameState.whosTurn = gameState.first;


                yield return new WaitUntil(NextTurnAndGameOver);

                players[0].CheckFindPairAndBombCard();
                //players[1].CheckFindPairAndBombCard();

                gameState.state = GameState.State.SecondTurn;


                if (players[gameState.second].state == SlotPosition.State.You)
                {
                    players[gameState.second].YourTurn();
                    gameState.auto = true;
                }
                else
                {
                    gameState.auto = false;
                }

                gameState.whosTurn = gameState.second;


                yield return new WaitUntil(NextTurnAndGameOver);
                               
                players[0].CheckFindPairAndBombCard();
                //players[1].CheckFindPairAndBombCard();

                if (players[0].myCardList.Count == 0 && players[1].myCardList.Count == 0)
                {
                    gameState.state = GameState.State.End;
                }

            }

        }

        private IEnumerator GameEnd()
        {

            WhosWinner();

            calculateWinnerPoint.CalculatePoint();

            yield return null;



   
        }

        private void ReceiveCard(Card _card)
        {
            StartCoroutine(ProgressCard(_card));
        }

        private IEnumerator ProgressCard(Card _card)
        {
            //폭탄이 돼서 덱카드를 뒤집어야하는 상황
            if (_card.infomation.kind == Infomation.Kind.None)
            {
                TurnEnd();

                players[gameState.whosTurn].bombBackCard.RemoveBackCard(_card);
                players[gameState.whosTurn].myCardList.Remove(_card);
            }
            else
            {
                yield return StartCoroutine(bomb.CheckBomb(_card, badak.badakCardList));

                foreach (var item in bomb.bombCardList)
                {
                    //MyTurnEnd 를 한번만 호출하기 위해서 
                    if (item.Equals(bomb.bombCardList.Last()))
                    {
                        badak.SortingBadakCardList(item, TurnEnd);
                        players[gameState.whosTurn].haveCardSlotManager.ReceiveHaveCards(item, badak, players[gameState.whosTurn].haveCardSlotManager.ReceiveMyCard);

                    }
                    else
                    {
                        badak.SortingBadakCardList(item);
                    }
                    players[gameState.whosTurn].myCardList.Remove(item);
                }

            }

            players[gameState.whosTurn].SortMyCard();

            //현재 카드 개수를 알아야 막판에 쪽, 따닥, 판쓸, 뻑을 넘긴다
            gameState.cardCount = players[gameState.whosTurn].myCardList.Count;
        }

        private void TurnEnd()
        {
            //폭탄이면 효과 보여줌
            bomb.StarBombEffect();
            
            //덱에서 카드 뽑음.
            Card _card = deck.PopDeckCard(DeckTurnEnd);
            players[gameState.whosTurn].haveCardSlotManager.ReceiveHaveCards(_card, badak, players[gameState.whosTurn].haveCardSlotManager.ReceiveDeckCard);

        }

        private void DeckTurnEnd()
        {
            if (bomb.isBomb == true)
            {
                bomb.isBomb = false;
                //뒷면 카드 표현
                players[gameState.whosTurn].myCardList.AddRange(players[gameState.whosTurn].bombBackCard.CreateBackCard());
                //카드 정렬
                players[gameState.whosTurn].SortMyCardList();
                //배율 
                players[gameState.whosTurn].haveCardSlotManager.calculateTotalPoint.square *= 2;
                //상대방 피 뺏음
                players[gameState.whosTurn].haveCardSlotManager.takeCardCount++;

                print("Bomb: " + players[gameState.whosTurn].haveCardSlotManager.calculateTotalPoint.square);
            }

            if (bomb.isShake == true)
            {
                bomb.isShake = false;
                //배율 
                players[gameState.whosTurn].haveCardSlotManager.calculateTotalPoint.square *= 2;

                print("Shake: " + players[gameState.whosTurn].haveCardSlotManager.calculateTotalPoint.square);
            }

            players[gameState.whosTurn].PlayerTurnEnd();
        }

        private bool NextTurnAndGameOver()
        {
            if (gameState.state.Equals(GameState.State.TurnEnd))
            {
                return true;
            }
            else if (gameState.state.Equals(GameState.State.End))
            {
                StopCoroutine("GameProgress");
                StopCoroutine("GamePlaying");
                StartCoroutine(GameEnd());

                return true;
            }

            return false;
        }

        //점수 비교 후 선 정함
        private void WhosWinner()
        {
            int _firstPoint = players[gameState.first].haveCardSlotManager.totalPoint;
            int _secondPoint = players[gameState.second].haveCardSlotManager.totalPoint;

            if (_firstPoint > _secondPoint)
            {
                gameState.winner = gameState.first;
                gameState.loser = gameState.second;

                if (players[gameState.first].state == SlotPosition.State.Me)
                {
                    gameState.first = 1;
                    gameState.second = 0;

                    print("Winner");
                }
                else
                {
                    gameState.first = 0;
                    gameState.second = 1;
                    print("Loser");
                }
            }
            else
            {
                gameState.winner = gameState.second;
                gameState.loser = gameState.first;

                if (players[gameState.second].state == SlotPosition.State.Me)
                {
                    print("Winner");
                    gameState.first = 1;
                    gameState.second = 0;

                }
                else
                {
                    gameState.first = 0;
                    gameState.second = 1;

                    print("Loser");
                }
            }

            players[gameState.first].myTurn = (int)GameState.State.FirstTurn;
            players[gameState.second].myTurn = (int)GameState.State.SecondTurn;

        }

        private void Initialized()
        {
            deck.Initialized();
            bomb.Initialized();
            badak.Initialized();
            foreach (var item in players)
            {
                item.initialized();
            }
        }

        public void Restart()
        {
            Initialized();

            StartCoroutine(GameProgress());

        }

    }

}

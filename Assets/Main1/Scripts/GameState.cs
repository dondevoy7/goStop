using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main1
{
    [CreateAssetMenu]
    public class GameState : ScriptableObject
    {
        public enum State { Idle, First, Distribution, FirstTurn, SecondTurn, GamePlaying, TurnEnd, End }
        [HideInInspector]
        public State state = State.Idle;
        [HideInInspector]
        public int first = 0;  //순서가 변경 되서 처음이 0일 수 있고 1일 수 있다
        [HideInInspector]
        public int second = 1;
        [HideInInspector]
        public int whosTurn = 0; //누구 차례인가?
        [HideInInspector]
        public bool auto = false;
        [HideInInspector]
        public int cardCount; // 가지고 있는 카드 개수
        [HideInInspector]
        public int winner; //누가 이겼는가?
        [HideInInspector]
        public int loser; // 누가 졌냐?

    }

}

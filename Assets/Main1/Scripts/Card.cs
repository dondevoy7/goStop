using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Events;

namespace Main1
{
    public class Card : MonoBehaviour
    {
        private enum State { Idle, Up, Rotate, Rotate1,  Move, End }
        private State state = State.Idle;
        private Vector3 targetPos; //목표점
        private Vector3 upTargetPos;
        private Quaternion targetQuaternion;
        private float degreeZ;

        public enum SlotSate { Me, You, Badak}
        private SlotSate slotState;
        public float speed;
        public AnimationCurve animationCurve;
        public Infomation infomation;
        [HideInInspector]
        public int position; //어느 번째 바닥에 위치하냐. 초기화 필요
        [HideInInspector]
        public int slotPosition; // 
        [HideInInspector]
        public SpriteRenderer[] spriteRenderer;
        //[HideInInspector]
        //public bool isDeck = false; //덱에서 뽑은 카드냐
        //[HideInInspector]
        //public bool isMySlot = false; //내가 낸 카드냐

        public event System.Action actionMoveEndCard;
        public event System.Action<Card> actionMoveEnd;

        static int sortingLayer = 0; //초기화 필요

        private void Awake()
        {
            spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        public void MoveMyOrYourSlot(Vector3 _targetPos, SlotSate _slotState, System.Action _actionMoveEndCard)
        {
            state = State.Up;
            targetPos = _targetPos;
            upTargetPos = new Vector3(transform.position.x, transform.position.y, -2.0f);
            degreeZ = 0.0f;
            targetQuaternion = Quaternion.Euler(0, 0, 0);
            actionMoveEndCard = _actionMoveEndCard;
            slotState = _slotState;
        }

        public void MoveBadakSlot(Vector3 _targetPos, int _num, SlotSate _slotState, System.Action _actionMoveBadak = null)
        {
            state = State.Up;
            targetPos = _targetPos;
            upTargetPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + -1.0f);
            degreeZ = Random.Range(-90.0f, 90.0f);
            position = _num;
            targetQuaternion = Quaternion.Euler(0, 0, degreeZ);
            slotState = _slotState;
            actionMoveEndCard = _actionMoveBadak;

        }

        public void MoveHaveSlot(Vector3 _targetPos, System.Action<Card> _action = null)
        {
            state = State.Move;
            targetPos = _targetPos;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            actionMoveEndCard = null; //혹시 몰라서 초기화
            actionMoveEnd = _action;
            spriteRenderer[0].sortingOrder = sortingLayer++;
        }

        public void MoveDeckSlot()
        {
            if (spriteRenderer[0] == null || spriteRenderer[1] == null)
                return;

            transform.position = new Vector3(-3f, 0, 0);
            actionMoveEndCard = null; //혹시 몰라서 초기화
            actionMoveEnd = null;
            spriteRenderer[0].sortingOrder = 0;
            spriteRenderer[1].sortingOrder = 0;
            sortingLayer = 0;
            transform.localRotation = new Quaternion(0, 180, 0, 0);
            //state = State.Move;
        }

        public void InitActionCard()
        {
            actionMoveEnd = null;
        }

        // Update is called once per frame
        void Update()
        {
            switch (state)
            {
                case State.Idle:
                    break;
                case State.Up:
                    float _timeUp = animationCurve.Evaluate(Time.deltaTime * speed);
                    transform.position = Vector3.Lerp(transform.position, upTargetPos, _timeUp);
                    float _dis = Vector3.Distance(transform.position, upTargetPos);
                    if (_dis <= 0.01f)
                    {
                        switch (slotState)
                        {
                            case SlotSate.Me:
                                state = State.Rotate;
                                break;
                            case SlotSate.You:
                                //spriteRenderer[1].sortingOrder = sortingLayer++;
                                state = State.Move;
                                break;
                            case SlotSate.Badak:
                                state = State.Rotate;
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case State.Rotate:
                    float _time = animationCurve.Evaluate(Time.deltaTime * 6f);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, _time);
                    if (transform.localRotation.y <= 0.1f)
                    {
                        spriteRenderer[0].sortingOrder = sortingLayer++;
                        transform.localRotation = new Quaternion(0, 0, 0, 0);
                        if (degreeZ != 0.0f)
                        {
                            state = State.Rotate1;
                        }
                        else
                        {
                            state = State.Move;
                        }
                    }
                    break;
                case State.Rotate1:
                    float _timed = Time.deltaTime * 6f;
                    float _timeRotate = animationCurve.Evaluate(_timed);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetQuaternion, _timeRotate);
                    if (_timed >= 0.09f)
                    {
                        state = State.Move;
                        //transform.localRotation = new Quaternion(0, 0, degreeZ, 0);
                    }
                    break;
                case State.Move:
                    float _timeMove = animationCurve.Evaluate(Time.deltaTime * speed);
                    transform.position = Vector2.Lerp(transform.position, targetPos, _timeMove);
                    float _distance = Vector2.Distance(transform.position, targetPos);
                    if (_distance <= 0.01f)
                    {
                        transform.position = targetPos;
                        state = State.End;
                    }
                    break;
                case State.End:
                    state = State.Idle;
                    actionMoveEndCard?.Invoke();
                    actionMoveEnd?.Invoke(this);
                    break;
                default:
                    break;
            }
        }
    }

}

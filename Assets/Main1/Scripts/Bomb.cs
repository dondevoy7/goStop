using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class Bomb : MonoBehaviour
    {
        public GameState gamestate;
        public Sprite bombSprite;
        public SelectCard selectCard;
        public ShowShakeCards showShakeCards;
        [HideInInspector]
        public bool isShake = false;
        [HideInInspector]
        public List<Card> bombCardList = new List<Card>();
        [HideInInspector]
        public bool isBomb = false;
        public ParticleSystem bombParticle;

        private Dictionary<int, List<Card>> bombDic = new Dictionary<int, List<Card>>();
        private List<List<Card>> bombList = new List<List<Card>>();
        private Vector2 bombPos;
        
        private void Start()
        {
        }

        public void Initialized()
        {
            isShake = false;
            bombDic.Clear();
        }

        public void BombCardDisplay(SpriteRenderer[] _spritePairs, List<Card> _myCardList)
        {
            int _count = 0;
            bombDic.Clear();

            for (int i = 1; i <= 12; i++)
            {
                var _list = (from month in _myCardList
                             where month.infomation.month == i
                             select month).ToList();

                if (_list.Count == 3)
                {
                    bombDic[_count++] = _list;
                }
            }
            //폭탄 표시
            foreach (var item in bombDic)
            {
                foreach (var i in item.Value)
                {
                    _spritePairs[i.slotPosition].gameObject.SetActive(true);
                    _spritePairs[i.slotPosition].sprite = bombSprite;
                }
            }

            //노가다 폭탄 계산
            {
                //int _bombCount = 0;
                //int _bombDicCount = 0;
                //List<Card> _temp = new List<Card>();

                //for (int k = 0; k < myCardList.Count; k++)
                //{
                //    _bombCount = 0;

                //    for (int j = k; j < myCardList.Count; j++)
                //    {
                //        if (myCardList[k].infomation.month == myCardList[j].infomation.month)
                //        {
                //            _bombCount++;
                //            if (_bombCount == 3)
                //            {
                //                for (int l = k; l < k + _bombCount; l++)
                //                {
                //                    spritePairs[l].sprite = bombSprite;
                //                    spritePairs[l].gameObject.SetActive(true);
                //                    _temp.Add(_testCardList[l]);
                //                }
                //                bombDic[_bombDicCount++] = _temp.ToList();
                //            }
                //            else if (_bombCount == 4)
                //            {
                //                spritePairs[j].sprite = bombSprite;
                //                spritePairs[j].gameObject.SetActive(true);
                //                _temp.Add(_testCardList[j]);
                //                bombDic[_bombDicCount - 1] = _temp.ToList();
                //            }
                //        }
                //        else
                //        {
                //            _temp.Clear();
                //            k = j - 1;
                //            break;
                //        }
                //    }
                //}

            }
        }

        public IEnumerator CheckBomb(Card _card, List<Card>_badakCardList)
        {
            bombCardList.Clear();

            bombList = bombDic.Values.Where(x => x.Find (y => y.Equals(_card))).ToList();

            if (bombList.Count > 0)
            {
                Card _pairCard = _badakCardList.Find(x => x.infomation.month.Equals(bombList[0][0].infomation.month));
                if (_pairCard == null) //흔들다
                {
                    List<Sprite> _sprList = new List<Sprite>();
                    foreach (var item in bombList[0])
                    {
                        _sprList.Add(item.spriteRenderer[0].sprite);
                    }

                    if (gamestate.auto == true)
                    {
                        isShake = true;
                        selectCard.waitCount = -1;
                    }
                    else
                    {
                        selectCard.ShowShakePanel(_sprList);
                    }

                    yield return new WaitWhile(() => selectCard.waitCount != -1);

                    if (isShake == true)
                    {
                        showShakeCards.ReceiveShakeCard(_sprList);

                        yield return showShakeCards.ShowShakeCard();
                    }
                    bombCardList.Add(_card);
                }
                else //폭탄이다
                {
                    bombCardList = bombList[0].ToList();
                    isBomb = true;
                    bombPos = _pairCard.transform.position;
                }
            }
            else
            {
                bombCardList.Add(_card);
            }

        }

        public void StarBombEffect()
        {
            if (isBomb)
                StartCoroutine(PlayBombEffect());
        }

        private IEnumerator PlayBombEffect()
        {
            bombParticle.gameObject.SetActive(true);
            Vector2 _bombPos = new Vector2(bombPos.x + 0.4f, bombPos.y);
            bombParticle.transform.position = _bombPos;
            yield return new WaitForSeconds(bombParticle.main.duration);
            bombParticle.gameObject.SetActive(false);
        }




    }

}

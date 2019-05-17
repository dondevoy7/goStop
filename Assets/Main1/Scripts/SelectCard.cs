using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Main1
{
    public class SelectCard : MonoBehaviour
    {
        [Header("Shake")]
        public GameObject shakePanel;
        public Image[] shakeImages;

        [Header("Select")]
        public GameObject selectPanel;
        public Button[] selectButtons;
        [HideInInspector]
        public int selectNum;

        [Header("Configuration")]
        public TextMeshProUGUI countText;
        public Bomb bomb;

        private List<Sprite> sprites = new List<Sprite>();
        private enum State { None, Shake, Select}
        private State state = State.None;
        [HideInInspector]
        public int waitCount;

        private void Start()
        {
        }

        private void OnEnable()
        {
            countText.gameObject.SetActive(false);
            waitCount = 5;
            StartCoroutine("WaitSelectPanel");
        }

        private void OnDisable()
        {
            shakePanel.SetActive(false);
            selectPanel.SetActive(false);
        }

        #region Shake
        public void ShowShakePanel(List<Sprite>_spriteList)
        {
            gameObject.SetActive(true);

            shakePanel.SetActive(true);
            state = State.Shake;
            sprites = _spriteList;

            int _count = 0;
            foreach (var item in _spriteList)
            {
                shakeImages[_count++].sprite = item;
            }

        }

        public void PushShakeBtn(int _num)
        {
            if (_num == 0)
            {
                //흔듬
                bomb.isShake = true;
            }
            else
            {
                //흔들지 않음
                bomb.isShake = false;
            }

            DisableGameObject();
        }

        #endregion Shake


        #region Select
        public void ShowSelectCard(List<Sprite> _spriteList)
        {
            gameObject.SetActive(true);

            selectPanel.SetActive(true);
            state = State.Select;

            int _cnt = 0;
            foreach (var item in _spriteList)
            {
                selectButtons[_cnt++].image.sprite = item;
            }
        }
        
        public void PushSelectCard(int _num)
        {
            selectNum = _num;
            DisableGameObject();
        }
        #endregion Select

        public IEnumerator WaitSelectPanel()
        {
            yield return new WaitForSeconds(3.0f);

            countText.gameObject.SetActive(true);

            while (waitCount >= 0)
            {
                yield return new WaitForSeconds(1.0f);
                --waitCount;
                countText.SetText("{0}", waitCount);
            }

            //자동으로 선택 한다
            switch (state)
            {
                case State.None:
                    print("None");
                    break;
                case State.Shake:
                    bomb.isShake = false;
                    print("Shake");
                    break;
                case State.Select:
                    selectNum = 1;
                    print("Select");
                    break;
                default:
                    break;
            }

            gameObject.SetActive(false);

        }

        private void DisableGameObject()
        {
            StopCoroutine("WaitSelectPanel");
            waitCount = -1;
            gameObject.SetActive(false);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main1
{
    public class ShowShakeCards : MonoBehaviour
    {
        public Image[] images;

        private void OnEnable()
        {
            StartCoroutine(ShowShakeCard());
        }
        // Start is called before the first frame update
        void Start()
        {
        }

        public void ReceiveShakeCard(List<Sprite> _spriteList)
        {
            gameObject.SetActive(true);

            for (int i = 0; i < images.Length; i++)
            {
                images[i].sprite = _spriteList[i];
            }
        }

        public IEnumerator ShowShakeCard()
        {
            yield return new WaitForSeconds(3.0f);

            gameObject.SetActive(false);
        }
    }
}

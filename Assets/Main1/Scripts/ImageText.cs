using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main1
{
    public class ImageText : MonoBehaviour
    {
        public enum ImageName { Chyok, Clear, Dadak, Fuk, Godori, Chodan, Chyeondan, Hongdan, Fivegwang }

        public GameObject[] imageTexts;


        // Start is called before the first frame update
        void Start()
        {

        }

        public void DisplayImageText(int _name, Vector2 _pos = default(Vector2))
        {
            StartCoroutine(ShowImageText(_name, _pos));
        }

        private IEnumerator ShowImageText(int _name, Vector2 _pos)
        {
            if (_pos.Equals(Vector2.zero) == false)
            {
                imageTexts[_name].transform.position = _pos;
            }

            imageTexts[_name].SetActive(true);

            yield return new WaitForSeconds(0.5f);

            imageTexts[_name].SetActive(false);
            imageTexts[_name].transform.position = new Vector3(0,0,0);

        }

    }

}

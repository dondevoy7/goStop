using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Main1
{
    public class TakePiCard : MonoBehaviour
    {
        public CalculatePi yourpiCards;
        [HideInInspector]
        public int count; //몇장 뺏았나?

        private CalculatePi mypiCard;
        // Start is called before the first frame update
        void Start()
        {
            mypiCard = GetComponent<CalculatePi>();
        }

        public void TakePi(int _amount)
        {
            List<Card> _piCard = yourpiCards.TakeAwayCard(_amount);
            count = _piCard.Count;

            foreach (var item in _piCard)
            {
                mypiCard.ReceiveCard(item);
            }
        }



    }

}

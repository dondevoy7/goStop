using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main1
{
    [System.Serializable]
    public class Infomation 
    {
        public enum Kind { None, Gwang, YulGut, Di, Pi }
        public enum SubKind { None, Bigwang, Godori, HongDan, ChyeongDan, ChoDan, BatCard}
        public Kind kind;
        public SubKind subKind;
        public int number;
        public int month;
        public int point;
    }
}

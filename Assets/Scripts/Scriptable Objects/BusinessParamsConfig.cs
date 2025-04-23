using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    [CreateAssetMenu(fileName = "BusinessParams", menuName = "ScriptableObjects/BusinessParams")]
    public class BusinessParamsConfig : ScriptableObject
    {
        public bool Purchased;
        public float ProfitDelay;
        public int Price;
        public int Profit;
    }
}



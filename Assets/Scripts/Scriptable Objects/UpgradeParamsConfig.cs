using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestClickerEcs
{
    [CreateAssetMenu(fileName = "UpgradeParams", menuName = "ScriptableObjects/UpgradeParams")]
    public class UpgradeParamsConfig : ScriptableObject
    {
        public int Price;
        public int Procent;
    }

}


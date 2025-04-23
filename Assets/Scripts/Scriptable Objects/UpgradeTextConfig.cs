using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestClickerEcs
{
    [CreateAssetMenu(fileName = "UpgradeText", menuName = "ScriptableObjects/UpgradeText")]
    public class UpgradeTextConfig : ScriptableObject
    {
        public string Name = "Upgrade";
        [Space]
        public string Description = "PROFIT: +{0}%";
        [Space]
        public string Price = "Price: {0}$";
        [Space]
        public string Purchased = "purchased";
    }

}

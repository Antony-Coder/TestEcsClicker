using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TestClickerEcs
{
    [CreateAssetMenu(fileName = "BusinessText", menuName = "ScriptableObjects/BusinessText")]
    public class BusinessTextConfig : ScriptableObject
    {
        public string Name = "Business";
        [Space]
        public string LevelLabel = "LVL";
        public string LevelValue = "{0}";
        [Space]
        public string ProfitLabel = "PROFIT";
        public string ProfitValue = "{0}$";
        [Space]
        public string LevelUpLabel = "LVL UP";
        public string LevelUpValue = "Price: {0}$";


    }
}






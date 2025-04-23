using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    [CreateAssetMenu(fileName = "BusinessWindowText", menuName = "ScriptableObjects/BusinessWindowText")]
    public class BusinessWindowTextConfig : ScriptableObject
    {
        public string Balance = "Balance: {0}$";
    }
}

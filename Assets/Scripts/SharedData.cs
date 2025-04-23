using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestClickerEcs
{
    public class SharedData
    {
        public Transform RootBusinesses { get; private set; }
        public BusinessView PrefabBusiness { get; private set; }
        public BusinessSettings[] Businesses { get; private set; }
        public BusinessWindowTextConfig BusinessWindowConfig { get; private set; }
        public BalanceView BalanceView { get; private set; }

        public SharedData(BalanceView balanceView, Transform rootBusinesses, BusinessView prefabBusiness, BusinessWindowTextConfig businessWindowConfig, BusinessSettings[] businesses)
        {
            BusinessWindowConfig = businessWindowConfig;
            RootBusinesses = rootBusinesses;
            PrefabBusiness = prefabBusiness;
            Businesses = businesses;
            BalanceView = balanceView;
        }
    }
}

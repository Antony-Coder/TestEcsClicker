using System;

namespace TestClickerEcs
{
    [Serializable]
    public class BusinessSettings
    {
        public BusinessParamsConfig Params;
        public BusinessTextConfig Text;
        public UpgradeSettings[] Upgrades;
    }
}

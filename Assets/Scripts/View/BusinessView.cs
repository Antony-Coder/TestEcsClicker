using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TestClickerEcs
{
    public class BusinessView : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private TMP_Text businessName;
        [SerializeField] private TMP_Text levelLabel;
        [SerializeField] private TMP_Text levelValue;
        [SerializeField] private TMP_Text profitLabel;
        [SerializeField] private TMP_Text profitValue;
        [SerializeField] private LevelUpView levelUp;
        [SerializeField] private UpgradeView[] upgrades;

        public Image Bar { get => bar;  }
        public TMP_Text BusinessName { get => businessName;  }
        public LevelUpView LevelUp { get => levelUp; }
        public UpgradeView[] Upgrades { get => upgrades;  }
        public TMP_Text ProfitLabel { get => profitLabel;  }
        public TMP_Text ProfitValue { get => profitValue; }
        public TMP_Text LevelLabel { get => levelLabel; }
        public TMP_Text LevelValue { get => levelValue;  }
    }
}


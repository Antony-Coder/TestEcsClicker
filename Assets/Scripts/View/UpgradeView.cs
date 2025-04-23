using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TestClickerEcs
{
    public class UpgradeView : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameUpgrade;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text price;
        [SerializeField] private Button button;

        public TMP_Text NameUpgrade { get => nameUpgrade;  }
        public TMP_Text Description { get => description;  }
        public TMP_Text Price { get => price;  }
        public Button Button { get => button;  }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TestClickerEcs
{
    public class LevelUpView : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelUpLabel;
        [SerializeField] private TMP_Text price;
        [SerializeField] private Button button;

        public TMP_Text LevelUpLabel { get => levelUpLabel;}
        public TMP_Text Price { get => price; }
        public Button Button { get => button; }

    }
}

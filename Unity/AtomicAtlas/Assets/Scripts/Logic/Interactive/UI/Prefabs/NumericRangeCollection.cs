
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Atlas.Logic
{
    public class NumericRangeCollection : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text labelText;

        private List<NumericRangeEntry> numericRanges = new();

        public int MaxValue { get; private set; }
        public string LabelPrefix { get; private set; }
        public string LabelPostfix { get; private set; }

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetLabelValues(string prefix, string postfix)
        {
            LabelPrefix = prefix;
            LabelPostfix = postfix;
        }

        public void SetNumericRanges(List<NumericRangeEntry> ranges, int maxValue)
        {
            numericRanges = ranges;
            MaxValue = maxValue;

            foreach (var range in ranges)
            {
                range.OnValueUpdate += OnValueChange;
            }

            OnValueChange();
        }

        public void OnValueChange()
        {
            MaxValue = 0;

            foreach (var range in numericRanges)
            {
                MaxValue += range.MaxValue;
            }

            labelText.text = $"{LabelPrefix}: {MaxValue}{LabelPostfix}";
        }
    }
}
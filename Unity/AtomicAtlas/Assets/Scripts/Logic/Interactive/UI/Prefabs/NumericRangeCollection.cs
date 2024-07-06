
using UnityEngine;
using System.Collections.Generic;
using TMPro;

namespace Atlas.Logic
{
    public class NumericRangeCollection : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private Transform containerRoot;

        private List<NumericRangeEntry> numericRanges = new();

        public Transform ContainerRoot => containerRoot;
        public int MaxValue { get; private set; }
        public string ValueWarning { get; private set; } = "WARNING: TOTAL EXCEEDS";
        public string LabelPrefix { get; private set; } = "TOTAL";
        public string LabelPostfix { get; private set; } = "%";

        private int currentValue;

        private const int padding = 50;
        private const int thickness = 45;

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

        public void SetMaxValue(int max)
        {
            MaxValue = max;
        }

        public void AddNumericRange(NumericRangeEntry numericRangeEntry)
        {
            numericRangeEntry.OnValueUpdate += OnRangeValueChange;
            numericRanges.Add(numericRangeEntry);

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(0, padding + (thickness * numericRanges.Count));

            OnRangeValueChange();
        }

        public void OnRangeValueChange()
        {
            currentValue = 0;

            foreach (var range in numericRanges)
            {
                currentValue += range.MaxValue;
            }

            if (currentValue > MaxValue)
            {
                labelText.text = $"{ValueWarning} {MaxValue}";
            }
            else
            {
                labelText.text = $"{LabelPrefix}: {currentValue}{LabelPostfix}";
            }
        }
    }
}
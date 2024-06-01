using UnityEngine;
using TMPro;
using Atlas.WorldGen;

namespace Atlas.Logic
{
    public class NumericRangeEntry : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private TMP_InputField minValueInput;

        [SerializeField]
        private TMP_InputField maxValueInput;

        public int MinValue { get; private set; }
        public int MaxValue { get; private set; }
        public IntRange IntRangeValue => new IntRange { Max=MaxValue, Min=MinValue };

        public delegate void OnValueChange();

        public event OnValueChange OnValueUpdate;

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetLabel(string label)
        {
            labelText.text = label;
        }

        public void SetValues(int min, int max)
        {
            MinValue = min;
            MaxValue = max;
            minValueInput.text = min.ToString();
            maxValueInput.text = max.ToString();
        }

        public void OnMinValueChange()
        {
            var text = minValueInput.text;
            if (text == string.Empty)
            {
                text = "0";
            }

            MinValue = Mathf.Max(int.Parse(text), 0);
            MaxValue = Mathf.Max(MaxValue, MinValue);
            maxValueInput.text = MaxValue.ToString();

            OnValueUpdate.Invoke();
        }

        public void OnMaxValueChange()
        {
            var text = maxValueInput.text;
            if (text == string.Empty)
            {
                text = "0";
            }

            MaxValue = Mathf.Min(int.Parse(text), int.MaxValue);
            MinValue = Mathf.Min(MinValue, MaxValue);
            minValueInput.text = MinValue.ToString();

            OnValueUpdate.Invoke();
        }
    }
}
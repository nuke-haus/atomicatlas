
using UnityEngine;
using TMPro;

namespace Atlas.Logic
{
    public class NumericEntry : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private TMP_InputField valueInput;

        public int Value { get; private set; }

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

        public void SetValue(int number)
        {
            Value = number;
            valueInput.text = number.ToString();
        }

        public void OnNumericValueChange()
        {
            var text = valueInput.text;
            if (text == string.Empty)
            {
                text = "0";
            }

            Value = int.Parse(text);

            OnValueUpdate?.Invoke();
        }
    }
}
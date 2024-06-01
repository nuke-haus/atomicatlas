
using UnityEngine;
using TMPro;

namespace Atlas.Logic
{
    public class StringEntry : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private TMP_InputField valueInput;

        public string Value { get; private set; }

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

        public void SetValue(string value)
        {
            Value = value;
            valueInput.text = value;
        }

        public void OnStringValueChange()
        {
            var text = valueInput.text;
            if (text == string.Empty)
            {
                text = "NONE";
            }

            Value = text;

            OnValueUpdate?.Invoke();
        }
    }
}
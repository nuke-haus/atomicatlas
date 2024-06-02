
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Atlas.Logic
{
    public class BooleanEntry : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private Toggle toggle;

        public bool Value { get; private set; }

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

        public void SetValue(bool value)
        {
            Value = value;
            toggle.isOn = value;
        }

        public void OnBoolValueChange()
        {
            Value = toggle.isOn;

            OnValueUpdate?.Invoke();
        }
    }
}
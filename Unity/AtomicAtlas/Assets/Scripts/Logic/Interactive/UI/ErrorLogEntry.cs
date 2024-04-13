
using UnityEngine;
using TMPro;

namespace Atlas.Logic
{
    public class ErrorLogEntry : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text errorText;

        [SerializeField]
        private TMP_Text logLevelText;

        void Start()
        {

        }

        void Update()
        {

        }

        public void SetText(string text, string levelText)
        {
            errorText.text = text;
            logLevelText.text = levelText;
        }
    } 
}

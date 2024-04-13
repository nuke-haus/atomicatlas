using UnityEngine;

namespace Atlas.Logic
{
    public class EditorMenuManager : MonoBehaviour
    {
        public static EditorMenuManager GlobalInstance
        {
            get;
            private set;
        }

        [SerializeField]
        private ProvinceEditor provinceEditor;

        [SerializeField]
        private CaveEditor caveEditor;

        [SerializeField]
        private ConnectionEditor connectionEditor;

        void Start()
        {
            GlobalInstance = this;
        }

        void Update()
        {

        }

        public void SetProvinceEditorPanelActive(InteractiveNode node)
        {
            provinceEditor.gameObject.SetActive(true);
            caveEditor.gameObject.SetActive(false);
            connectionEditor.gameObject.SetActive(false);

            provinceEditor.Activate(node);
        }

        public void SetCaveEditorPanelActive(InteractiveNode node)
        {
            provinceEditor.gameObject.SetActive(false);
            caveEditor.gameObject.SetActive(true);
            connectionEditor.gameObject.SetActive(false);

            caveEditor.Activate(node);
        }

        public void SetConnectionEditorPanelActive(InteractiveConnection connection)
        {
            provinceEditor.gameObject.SetActive(false);
            caveEditor.gameObject.SetActive(false);
            connectionEditor.gameObject.SetActive(true);

            connectionEditor.Activate(connection);
        }
    } 
}

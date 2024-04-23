using UnityEngine;
using UnityEngine.UI;
using Atlas.WorldGen;

namespace Atlas.Logic
{
    public class ConnectionEditor : MonoBehaviour
    {
        [SerializeField]
        private Toggle NormalToggle;

        [SerializeField]
        private Toggle RiverToggle;

        [SerializeField]
        private Toggle MountainToggle;

        [SerializeField]
        private Toggle MountainPassToggle;

        private InteractiveConnection selectedConnection;

        void Start()
        {

        }

        void Update()
        {

        }

        public void HidePanel()
        {
            selectedConnection?.SetOutlineVisible(false);

            gameObject.SetActive(false);
        }

        public void Activate(InteractiveConnection connection)
        {
            if (selectedConnection != null)
            {
                selectedConnection.SetOutlineVisible(false);
            }

            selectedConnection = connection;
            selectedConnection.SetOutlineVisible(true);

            UpdatePanel();
        }

        public void ApplyChanges()
        {
            var connection = ConnectionType.STANDARD;

            if (RiverToggle.isOn)
            {
                connection = ConnectionType.RIVER;
            }
            if (MountainToggle.isOn)
            {
                connection = ConnectionType.MOUNTAIN;
            }
            if (MountainPassToggle.isOn)
            {
                connection = ConnectionType.MOUNTAIN_PASS;
            }

            selectedConnection.Connection.SetConnectionType(connection);

            UpdatePanel();
        }

        private void UpdatePanel()
        {
            NormalToggle.isOn = selectedConnection.Connection.ConnectionType == ConnectionType.STANDARD;
            RiverToggle.isOn = selectedConnection.Connection.ConnectionType == ConnectionType.RIVER;
            MountainToggle.isOn = selectedConnection.Connection.ConnectionType == ConnectionType.MOUNTAIN;
            MountainPassToggle.isOn = selectedConnection.Connection.ConnectionType == ConnectionType.MOUNTAIN_PASS;
        }
    }

}
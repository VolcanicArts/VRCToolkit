using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;

namespace VRCToolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class RadialUIButtonController : UdonSharpBehaviour
    {

        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")] [Tooltip("The buttons in the radial container")]
        public Image[] buttons;

        [Tooltip("The color of the selected button")]
        public Color selected = new Color(0, 1, 0, 1);

        [Tooltip("The color of the non-selected buttons")]
        public Color deselected = new Color(1, 0, 0, 1);

        [Tooltip("The initial button selected")]
        public int initialButton;

        [UdonSynced] private int _selectedButton;

        public void Start()
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
            _selectedButton = initialButton;
            UpdateButtons();
        }

        private void UpdateOwner()
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject))
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
            }
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            if (syncOverNetwork && player.IsValid() && player.isLocal) UpdateButtons();
        }

        public override void OnDeserialization()
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) UpdateButtons();
        }

        public void Mode0()
        {
            _selectedButton = 0;
            UpdateOwner();
            UpdateButtons();
        }

        public void Mode1()
        {
            _selectedButton = 1;
            UpdateOwner();
            UpdateButtons();
        }

        public void Mode2()
        {
            _selectedButton = 2;
            UpdateOwner();
            UpdateButtons();
        }

        public void Mode3()
        {
            _selectedButton = 3;
            UpdateOwner();
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].color = (i == _selectedButton) ? selected : deselected;
            }

            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }

    }
}
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCToolkit.UdonBehaviours.Controllers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BooleanTextController : UdonSharpBehaviour
    {
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")]
        [Tooltip(
            "The initial value of the boolean state")]
        public bool initialValue;

        public string stateOnText;

        public string stateOffText;

        public Color stateOnColor;

        public Color stateOffColor;

        private TextMeshProUGUI _textMeshProUGUI;
        [UdonSynced] private bool _state;

        public void Start()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            if (syncOverNetwork && !Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
            _state = initialValue;
            UpdateTextMeshPro();
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
            if (syncOverNetwork && player.IsValid() && player.isLocal) UpdateTextMeshPro();
        }

        public override void OnDeserialization()
        {
            UpdateTextMeshPro();
        }

        public void False()
        {
            SetState(false);
        }

        public void True()
        {
            SetState(true);
        }

        public void Toggle()
        {
            SetState(!_state);
        }

        private void SetState(bool state)
        {
            _state = state;
            UpdateOwner();
            UpdateTextMeshPro();
        }

        private void UpdateTextMeshPro()
        {
            _textMeshProUGUI.text = _state ? stateOnText : stateOffText;
            _textMeshProUGUI.color = _state ? stateOnColor : stateOffColor;
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
}
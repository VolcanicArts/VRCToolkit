using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCToolkit.UdonBehaviours.Controllers
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ObjectStateController : UdonSharpBehaviour
    {
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")] [Tooltip("The array of objects you want to control")]
        public GameObject[] objects;

        private bool[] _objectStates;

        private void Start()
        {
            if (syncOverNetwork && !Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
            _objectStates = new bool[objects.Length];
            RetrieveObjectStates();
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
            if (syncOverNetwork && player.IsValid() && player.isLocal) UpdateObjectStates();
        }

        public override void OnDeserialization()
        {
            UpdateObjectStates();
        }

        public void False()
        {
            UpdateOwner();
            SetObjectStates(false);
        }

        public void True()
        {
            UpdateOwner();
            SetObjectStates(true);
        }

        public void Toggle()
        {
            UpdateOwner();
            ToggleObjectStates();
        }

        private void RetrieveObjectStates()
        {
            for (int i = 0; i < _objectStates.Length; i++)
            {
                _objectStates[i] = objects[i].activeSelf;
            }
        }

        private void SetObjectStates(bool state)
        {
            for (int i = 0; i < _objectStates.Length; i++)
            {
                _objectStates[i] = state;
            }

            UpdateObjectStates();
        }

        private void ToggleObjectStates()
        {
            for (int i = 0; i < _objectStates.Length; i++)
            {
                _objectStates[i] = !_objectStates[i];
            }

            UpdateObjectStates();
        }

        private void UpdateObjectStates()
        {
            for (int i = 0; i < _objectStates.Length; i++)
            {
                objects[i].SetActive(_objectStates[i]);
            }

            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
}
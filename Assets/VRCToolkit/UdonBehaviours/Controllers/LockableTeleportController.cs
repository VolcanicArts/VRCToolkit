using UnityEngine;
#if VRC_SDK_VRCSDK3
using VRC.SDKBase;
#endif
#if UDON
using UdonSharp;
#endif

namespace VRCToolkit.UdonBehaviours.Controllers
{
    #if UDON
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class LockableTeleportController : UdonSharpBehaviour
    {
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")]
        [Tooltip(
            "The initial value of the locked state")]
        public bool initialValue;

        [Tooltip("The location to teleport the player to on interact")]
        public Transform teleportLocation;

        [UdonSynced] private bool _locked;

        public void Start()
        {
            if (syncOverNetwork && !Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
            _locked = initialValue;
            UpdateLock();
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
            if (syncOverNetwork && player.IsValid() && player.isLocal) UpdateLock();
        }

        public override void OnDeserialization()
        {
            UpdateLock();
        }

        public void Unlock()
        {
            SetLocked(false);
        }

        public void Lock()
        {
            SetLocked(true);
        }

        public void ToggleLock()
        {
            SetLocked(!_locked);
        }

        private void SetLocked(bool locked)
        {
            _locked = locked;
            UpdateOwner();
            UpdateLock();
        }

        private void UpdateLock()
        {
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }

        public override void Interact()
        {
            if (!_locked) Networking.LocalPlayer.TeleportTo(teleportLocation.position, teleportLocation.rotation);
        }
    }
    #endif
}
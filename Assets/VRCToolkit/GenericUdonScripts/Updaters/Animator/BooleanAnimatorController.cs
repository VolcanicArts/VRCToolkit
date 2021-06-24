using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCToolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class BooleanAnimatorController : UdonSharpBehaviour
    {
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")] [Tooltip("The animator this script is controlling")]
        public Animator animator;

        [Tooltip("The boolean this script is updating")]
        public string booleanName;

        [Tooltip(
            "The initial value of the boolean. This overrides the value in the animator parameter window on Start")]
        public bool initialValue;

        [UdonSynced] private bool _mode;

        public void Start()
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
            _mode = initialValue;
            UpdateAnimator();
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
            if (syncOverNetwork && player.IsValid() && player.isLocal) UpdateAnimator();
        }

        public override void OnDeserialization()
        {
            if (!Networking.IsOwner(Networking.LocalPlayer, gameObject)) UpdateAnimator();
        }

        public void False()
        {
            _mode = false;
            UpdateOwner();
            UpdateAnimator();
        }

        public void True()
        {
            _mode = true;
            UpdateOwner();
            UpdateAnimator();
        }

        public void Toggle()
        {
            _mode = !_mode;
            UpdateOwner();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetBool(booleanName, _mode);
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
}
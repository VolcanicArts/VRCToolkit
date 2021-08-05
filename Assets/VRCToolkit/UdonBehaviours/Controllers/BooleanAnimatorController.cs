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
    public class BooleanAnimatorController : UdonSharpBehaviour
    {
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")] [Tooltip("The animator this script is controlling")]
        public Animator animator;

        [Tooltip("The boolean animator parameter this script is updating")]
        public string booleanName;

        [Tooltip(
            "The initial value of the boolean. This overrides the value in the animator parameter window on Start")]
        public bool initialValue;

        [UdonSynced] private bool _mode;

        public void Start()
        {
            if (syncOverNetwork && !Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
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
            UpdateAnimator();
        }

        public void False()
        {
            SetMode(false);
        }

        public void True()
        {
            SetMode(true);
        }

        public void Toggle()
        {
            SetMode(!_mode);
        }

        private void SetMode(bool value)
        {
            _mode = value;
            UpdateOwner();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetBool(booleanName, _mode);
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
    #endif
}
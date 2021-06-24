
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCToolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class IntegerAnimatorController : UdonSharpBehaviour
    {

        private const int minMode = 0;
        private const int maxMode = 3;
        
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")] [Tooltip("The animator this script is controlling")]
        public Animator animator;

        [Tooltip("The integer this script is updating")]
        public string integerName;

        [Tooltip(
            "The initial value of the integer. This overrides the value in the animator parameter window on Start")]
        public int initialValue;

        [UdonSynced] private int _mode;

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

        public void Value0()
        {
            _mode = 0;
            UpdateOwner();
            UpdateAnimator();
        }

        public void Value1()
        {
            _mode = 1;
            UpdateOwner();
            UpdateAnimator();
        }

        public void Value2()
        {
            _mode = 2;
            UpdateOwner();
            UpdateAnimator();
        }

        public void Value3()
        {
            _mode = 3;
            UpdateOwner();
            UpdateAnimator();
        }

        public void Increase()
        {
            _mode += 1;
            UpdateOwner();
            UpdateAnimator();
        }
        
        public void Decrease()
        {
            _mode -= 1;
            UpdateOwner();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            _mode = Mathf.Clamp(_mode, minMode, maxMode);
            animator.SetInteger(integerName, _mode);
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
}
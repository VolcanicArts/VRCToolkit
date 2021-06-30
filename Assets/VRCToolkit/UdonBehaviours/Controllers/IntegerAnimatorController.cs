using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCToolkit.UdonBehaviours.Controllers
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

        [Tooltip("The integer animator parameter this script is updating")]
        public string integerName;

        [Tooltip(
            "The initial value of the integer. This overrides the value in the animator parameter window on Start")]
        public int initialValue;

        [UdonSynced] private int _mode;

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

        public void Value0()
        {
            SetValue(0);
        }

        public void Value1()
        {
            SetValue(1);
        }

        public void Value2()
        {
            SetValue(2);
        }

        public void Value3()
        {
            SetValue(3);
        }

        public void Increase()
        {
            SetValue(_mode + 1);
        }

        public void Decrease()
        {
            SetValue(_mode - 1);
        }

        private void SetValue(int value)
        {
            _mode = _mode = Mathf.Clamp(value, minMode, maxMode);
            UpdateOwner();
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            animator.SetInteger(integerName, _mode);
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
}
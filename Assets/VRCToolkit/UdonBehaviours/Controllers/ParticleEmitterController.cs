using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

namespace VRCToolkit.UdonBehaviours.Controllers
{
    [RequireComponent(typeof(ParticleSystem))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ParticleEmitterController : UdonSharpBehaviour
    {
        [Header("Network")]
        [Tooltip(
            "If true, this script will sync the object states from the owner of the script receiving events. If false, each event will run locally")]
        public bool syncOverNetwork = true;

        [Header("Attributes")]
        [Tooltip("When off, how many particles should the particle system emit")]
        public float offAmount;
        
        [Tooltip("When on, how many particles should the particle system emit")]
        public float onAmount;

        [Tooltip("Should the particle system be on or off on Start. This overrides any emission set in the inspector")]
        public bool initialState;

        private ParticleSystem _particleSystem;
        [UdonSynced] private bool _mode;

        public void Start()
        {
            _particleSystem = (ParticleSystem) GetComponent(typeof(ParticleSystem));
            if (syncOverNetwork && !Networking.IsOwner(Networking.LocalPlayer, gameObject)) return;
            _mode = initialState;
            UpdateEmitter();
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
            if (syncOverNetwork && player.IsValid() && player.isLocal) UpdateEmitter();
        }

        public override void OnDeserialization()
        {
            UpdateEmitter();
        }

        public void Off()
        {
            SetMode(false);
        }

        public void On()
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
            UpdateEmitter();
        }

        private void UpdateEmitter()
        {
            if (_particleSystem == null) return;
            var emission = _particleSystem.emission;
            emission.rateOverTime = _mode ? onAmount : offAmount;
            if (syncOverNetwork && Networking.IsOwner(Networking.LocalPlayer, gameObject)) RequestSerialization();
        }
    }
}
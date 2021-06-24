using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace VRCToolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class GenericEventSenderTrigger : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")]
        public UdonBehaviour eventReceiver;

        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver on pickup")] [CanBeNull]
        public string onPlayerTriggerEnterEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use down")] [CanBeNull]
        public string onPlayerTriggerExitEventName;

        [Tooltip("OnPlayerTriggerEvents get sent by VRC globally by default. This attribute filters it so only the local player processes the event")]
        public bool filterLocalPlayer = true;

        public override void OnPlayerTriggerEnter(VRCPlayerApi player)
        {
            if (filterLocalPlayer && !player.isLocal) return;
            HandleSendingEvent(onPlayerTriggerEnterEventName);
        }

        public override void OnPlayerTriggerExit(VRCPlayerApi player)
        {
            if (filterLocalPlayer && !player.isLocal) return;
            HandleSendingEvent(onPlayerTriggerExitEventName);
        }

        private void HandleSendingEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
            eventReceiver.SendCustomEvent(eventName);
        }
    }
}
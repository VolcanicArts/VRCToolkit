using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace VRCToolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class GenericEventSenderPickup : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")]
        public UdonBehaviour eventReceiver;

        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver on pickup")] [CanBeNull]
        public string onPickupEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use down")] [CanBeNull]
        public string onPickupUseDownEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use up")] [CanBeNull]
        public string onPickupUseUpEventName;

        public override void OnPickup()
        {
            HandleSendingEvent(onPickupEventName);
        }

        public override void OnPickupUseDown()
        {
            HandleSendingEvent(onPickupUseDownEventName);
        }

        public override void OnPickupUseUp()
        {
            HandleSendingEvent(onPickupUseUpEventName);
        }

        private void HandleSendingEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
            eventReceiver.SendCustomEvent(eventName);
        }
    }
}
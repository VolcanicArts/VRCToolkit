using JetBrains.Annotations;
using UnityEngine;
using VRC.Udon;
using UdonSharp;

namespace VRCToolkit.UdonBehaviours.EventSenders
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class PickupEventSender : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")] [CanBeNull]
        public UdonBehaviour eventReceiver;

        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver on pickup")] [CanBeNull]
        public string onPickupEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use down")] [CanBeNull]
        public string onPickupUseDownEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use up")] [CanBeNull]
        public string onPickupUseUpEventName;

        [Tooltip("The event you want to send to the eventReceiver on drop")] [CanBeNull]
        public string onDropEventName;

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

        public override void OnDrop()
        {
            HandleSendingEvent(onDropEventName);
        }

        private void HandleSendingEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
            if (eventReceiver == null) return;
            eventReceiver.SendCustomEvent(eventName);
        }
    }
}
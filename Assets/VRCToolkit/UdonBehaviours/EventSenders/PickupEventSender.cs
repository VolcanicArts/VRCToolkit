using UnityEngine;
#if UDON
using VRC.Udon;
using UdonSharp;
#endif

namespace VRCToolkit.UdonBehaviours.EventSenders
{
    #if UDON
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class PickupEventSender : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")]
        public UdonBehaviour eventReceiver;

        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver on pickup")]
        public string onPickupEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use down")]
        public string onPickupUseDownEventName;

        [Tooltip("The event you want to send to the eventReceiver on pickup use up")]
        public string onPickupUseUpEventName;

        [Tooltip("The event you want to send to the eventReceiver on drop")]
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
    #endif
}
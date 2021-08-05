using UnityEngine;
#if UDON
using VRC.Udon;
using UdonSharp;
#endif

namespace VRCToolkit.UdonBehaviours.EventSenders
{
    #if UDON
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class InteractEventSender : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")]
        public UdonBehaviour eventReceiver;

        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver on interact")]
        public string onInteractEventName;

        public override void Interact()
        {
            HandleSendingEvent(onInteractEventName);
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
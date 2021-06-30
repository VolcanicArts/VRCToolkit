using JetBrains.Annotations;
using UnityEngine;
using VRC.Udon;
using UdonSharp;

namespace VRCToolkit.UdonBehaviours.EventSenders
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
    public class InteractEventSender : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")] [CanBeNull]
        public UdonBehaviour eventReceiver;

        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver on interact")] [CanBeNull]
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
}
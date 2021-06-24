using JetBrains.Annotations;
using UdonSharp;
using UnityEngine;
using VRC.Udon;

namespace VRCToolkit
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class GenericEventSenderInteract : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")]
        public UdonBehaviour eventReceiver;
        
        [Header("Available events")] [Tooltip("The event you want to send to the eventReceiver")] [CanBeNull]
        public string onInteractEventName;

        public override void Interact()
        {
            HandleSendingEvent(onInteractEventName);
        }
        
        private void HandleSendingEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
            eventReceiver.SendCustomEvent(eventName);
        }
    }

}
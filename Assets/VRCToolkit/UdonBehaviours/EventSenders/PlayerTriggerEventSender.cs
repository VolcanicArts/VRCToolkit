using UnityEngine;
#if VRC_SDK_VRCSDK3
using VRC.SDKBase;
#endif
#if UDON
using VRC.Udon;
using UdonSharp;
#endif

namespace VRCToolkit.UdonBehaviours.EventSenders
{
    #if UDON
    [UdonBehaviourSyncMode(BehaviourSyncMode.NoVariableSync)]
    public class PlayerTriggerEventSender : UdonSharpBehaviour
    {
        [Header("Receiver")] [Tooltip("The receiver of the events")]
        public UdonBehaviour eventReceiver;

        [Header("Available events")]
        [Tooltip("The event you want to send to the eventReceiver on player trigger enter")]
        public string onPlayerTriggerEnterEventName;

        [Tooltip("The event you want to send to the eventReceiver on player trigger exit")]
        public string onPlayerTriggerExitEventName;

        [Tooltip("The event you want to send to the eventReceiver on player trigger stay")]
        public string onPlayerTriggerStayEventName;

        [Header("Settings")]
        [Tooltip(
            "OnPlayerTriggerEvents get sent by VRC globally by default. This attribute filters it so only the local player that triggered the event processes the event")]
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

        public override void OnPlayerTriggerStay(VRCPlayerApi player)
        {
            if (filterLocalPlayer && !player.isLocal) return;
            HandleSendingEvent(onPlayerTriggerStayEventName);
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
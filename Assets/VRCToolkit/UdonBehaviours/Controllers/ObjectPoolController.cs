using UnityEngine;
#if VRC_SDK_VRCSDK3
using VRC.SDK3.Components;
using VRC.SDKBase;
#endif
#if UDON
using UdonSharp;
#endif

namespace VRCToolkit.UdonBehaviours.Controllers
{
    #if UDON
    [RequireComponent(typeof(VRCObjectPool))]
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class ObjectPoolController : UdonSharpBehaviour
    {
        private VRCObjectPool _objectPool;

        public void Start()
        {
            _objectPool = (VRCObjectPool) gameObject.GetComponent(typeof(VRCObjectPool));
            SpawnAllObjectsInPool();
        }

        public void Return()
        {
            if (!Networking.LocalPlayer.isMaster) return;
        
            foreach (var obj in _objectPool.Pool)
            {
                var objectSync = (VRCObjectSync) obj.GetComponent(typeof(VRCObjectSync));
                objectSync.FlagDiscontinuity();
                _objectPool.Return(obj);
            }

            SpawnAllObjectsInPool();
        }

        private void SpawnAllObjectsInPool()
        {
            GameObject obj;
            do
            {
                obj = _objectPool.TryToSpawn();
            } while (obj != null);
        }
    }
    #endif
}
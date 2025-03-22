using System;
using UnityEngine;

namespace Util
{
    public class MonoUpdater : MonoBehaviour
    {
        public static Action OnUpdate;
        
        
        private void Update()
        {
            OnUpdate?.Invoke();
        }
    }
}
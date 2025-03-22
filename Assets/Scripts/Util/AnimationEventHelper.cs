using System;
using UnityEngine;

namespace Util
{
    public class AnimationEventHelper : MonoBehaviour
    {
        public Action<String> OnAnim;
        public Action<String> OnSound;
        
        private void OnAnimEvent(String eventName)
        {
            OnAnim?.Invoke(eventName);
        }
        
        private void OnSoundEvent(string soundPath)
        {
            OnSound?.Invoke(soundPath);
        }
    }
}
using UnityEngine;

namespace GameTemplate.Scripts.Systems.CursorLock
{
    public class CursorLocker
    {
        public enum CursorState
        {
            Locked,
            Unlocked,
        }
        
        public CursorState ActiveState = CursorState.Locked;

        public void SetCursorLock(bool lockState)
        {
            if (lockState)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
        
        void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            ActiveState = CursorState.Locked;
        }

        void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            // Optionally, make it explicitly visible when unlocked
            Cursor.visible = true;
            ActiveState = CursorState.Unlocked;
        }
    }
}
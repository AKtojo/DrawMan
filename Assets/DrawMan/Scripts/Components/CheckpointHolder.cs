using UnityEngine;

namespace DrawMan.Core
{
    public class CheckpointHolder : MonoBehaviour
    {
        private Transform checkpoint = null;

        public void TeleportToCheckpoint()
        {
            transform.position = checkpoint.position;
            transform.rotation = checkpoint.rotation;
        }

        public void ResetCheckpoint()
        {
            checkpoint = null;
        }

        public void SetCheckpoint(Transform checkpoint)
        {
            this.checkpoint = checkpoint;
        }
    }
}

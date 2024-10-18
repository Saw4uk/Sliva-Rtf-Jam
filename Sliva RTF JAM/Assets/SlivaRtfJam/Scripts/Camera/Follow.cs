using UnityEngine;

namespace SlivaRtfJam.Scripts
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private GameObject followedObject;

        public void LateUpdate()
        {
            var followedPosition = followedObject.transform.position;
            transform.position =
                new Vector3(
                    followedPosition.x,
                    followedPosition.y,
                    transform.position.z);
        }
    }
}
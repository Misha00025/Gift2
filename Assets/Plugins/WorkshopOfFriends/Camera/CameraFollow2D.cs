using UnityEngine;


namespace Wof.Cameras
{
    public class CameraFollow2D : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target;              // Объект, за которым следит камера
        [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Смещение камеры (для 2D обычно z = -10)

        [Header("Follow Parameters")]
        [SerializeField] private float speed = 4f;


        private void Update()
        {
            if (target == null)
                return;

            var position = Vector3.Lerp(transform.position, target.position, Time.deltaTime*speed);
            position.z = offset.z;
            transform.position = position;
        }
    }
}

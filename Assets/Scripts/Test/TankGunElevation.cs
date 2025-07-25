using UnityEngine;

namespace Test
{
    public class TankGunElevation : MonoBehaviour
    {
        [Header("Gun Settings")]
        public Transform gun; // Ссылка на трансформ пушки
        public float elevationSpeed = 2f; // Скорость подъема/опускания
        public float maxElevationAngle = 15f; // Максимальный угол подъема
        public float maxDepressionAngle = -50f; // Максимальный угол опускания
        public float smoothTime = 0.1f; // Время сглаживания движения

        [Header("Mouse Settings")]
        public float mouseSensitivity = 1f;
        public bool lockCursor = true;

        private float currentElevation = 0f;
        private float velocityY = 0f;

        private void Start()
        {
            if (!lockCursor) return;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // Получаем ввод мыши по вертикали
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Вычисляем новое значение угла подъема
            float targetElevation = currentElevation + mouseY * elevationSpeed;
        
            // Ограничиваем угол подъема/опускания
            targetElevation = Mathf.Clamp(targetElevation, maxDepressionAngle, maxElevationAngle);

            // Плавное изменение угла
            currentElevation = Mathf.SmoothDamp(
                currentElevation, 
                targetElevation, 
                ref velocityY, 
                smoothTime
            );

            // Применяем поворот к пушке (по оси X)
            gun.localEulerAngles = new Vector3(currentElevation, 0f, 0f);
        }

        private void OnDisable()
        {
            // Разблокируем курсор при отключении скрипта
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
using TMPro;
using UnityEngine;

namespace UI
{
    public class Tooltip : MonoBehaviour
    {
    
        [SerializeField] private RectTransform background;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private RectTransform textTransform;
        private const float PADDING = 2;
        
        public void SetText(string str)
        {
            text.text = str;
            background.sizeDelta = new Vector2(text.preferredWidth + PADDING*2, text.preferredHeight + PADDING*2);
        }

        private void Start()
        {
            textTransform.GetComponent<RectTransform>().localPosition += new Vector3(PADDING, PADDING, 0);
            SetPosition();
        }

        private void SetPosition() => transform.position = Input.mousePosition;

        private void Update() => SetPosition();
    }
}

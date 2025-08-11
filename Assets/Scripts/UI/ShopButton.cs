using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Managers
{
    public class ShopButton : MonoBehaviour, IPointerClickHandler
    {
        private bool opened = true;
        private Vector2 openedPosition;
        private Vector2 closedPosition;
        private bool moving;
        private RectTransform rectTransform;
        
        [SerializeField] private GameObject shopObject;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            var shopTransform = shopObject.GetComponent<RectTransform>();
            openedPosition = shopTransform.anchoredPosition;
            closedPosition = openedPosition + new Vector2(shopTransform.sizeDelta.x, 0);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (moving) return;
            moving = true;
            rectTransform.localScale *= new Vector2(-1, 1);
            SmoothMover.RunRect(shopObject, 10, opened ? closedPosition : openedPosition, 
                () => moving = false);
            opened = !opened;
        }
    }
}




















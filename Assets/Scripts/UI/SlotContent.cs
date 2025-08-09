using Item;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class SlotContent : MonoBehaviour
    {
        public Image image => gameObject.GetComponent<Image>();

        public void Destroy() => Destroy(gameObject);
    }
}
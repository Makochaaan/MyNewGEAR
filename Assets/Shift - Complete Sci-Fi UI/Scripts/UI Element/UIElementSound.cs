using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Michsky.UI.Shift
{
    [ExecuteInEditMode]
    public class UIElementSound : MonoBehaviour, IPointerEnterHandler,ISelectHandler,IPointerClickHandler,ISubmitHandler
    {
        [Header("Custom SFX")]
        public AudioClip hoverSFX;
        public AudioClip clickSFX;

        [Header("Settings")]
        public bool enableHoverSound = true;
        public bool enableClickSound = true;
        public bool checkForInteraction = true;

        private Button sourceButton;

        void OnEnable()
        {
            if (checkForInteraction == true) { sourceButton = gameObject.GetComponent<Button>(); }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;

            if (enableHoverSound == true)
            {
                if (hoverSFX == null)
                {
                    SEManager.SharedInstance.PlaySE("UIHoverDefault", false, Vector3.zero);
                }
                else
                {
                    SEManager.SharedInstance.PlaySE("UIHoverCustom", false, Vector3.zero);
                }
            }
        }
        public void OnSelect(BaseEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;

            if (enableHoverSound == true)
            {
                if (hoverSFX == null)
                {
                    SEManager.SharedInstance.PlaySE("UIHoverDefault", false, Vector3.zero);
                }
                else
                {
                    SEManager.SharedInstance.PlaySE("UIHoverCustom", false, Vector3.zero);
                }
            }
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;

            if (enableClickSound == true)
            {
                if (clickSFX == null)
                {
                    SEManager.SharedInstance.PlaySE("UIClickDefault", false, Vector3.zero);
                }
                else
                {
                    SEManager.SharedInstance.PlaySE("UIClickCustom", false, Vector3.zero);
                }
            }
        }
        public void OnSubmit(BaseEventData eventData)
        {
            if (checkForInteraction == true && sourceButton != null && sourceButton.interactable == false)
                return;

            if (enableClickSound == true)
            {
                if (clickSFX == null)
                {
                    SEManager.SharedInstance.PlaySE("UIClickDefault", false, Vector3.zero);
                }
                else
                {
                    SEManager.SharedInstance.PlaySE("UIClickCustom", false, Vector3.zero);
                }
            }
        }
    }
}
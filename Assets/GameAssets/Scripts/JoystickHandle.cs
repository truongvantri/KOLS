using System.Collections;
using System.Collections.Generic;
using FarmingEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KOLS
{
    public class JoystickHandle : MonoBehaviour
    {
        public static JoystickHandle Instance;
        
        public Canvas canvas;
        public RectTransform rectJoystickParent;
        public RectTransform pin;

        private CanvasGroup canvasGroup;
        private RectTransform rect;
        private bool m_isHold = false;
        private float m_distance = 75;
        private Vector2 m_dir;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this.gameObject);
                return;
            }
            
            canvasGroup = GetComponent<CanvasGroup>();
            rect = GetComponent<RectTransform>();
            canvasGroup.alpha = 1f;

            //if (!TheGame.IsMobile())
            //    enabled = false;
        }

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("IsPointerOverGameObject");
                return;
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                canvasGroup.alpha = 1;
                m_isHold = true;
                Vector2 posConvert = ScreenPointToCanvasPos(rectJoystickParent,Input.mousePosition);
                rect.anchoredPosition = posConvert;
            }

            if (Input.GetMouseButtonUp(0))
                m_isHold = false;

            if (!m_isHold)
            {
                m_dir = Vector2.zero;
                pin.anchoredPosition = m_dir;
                canvasGroup.alpha = 0;
                return;
            }

            Vector2 screenPos = ScreenPointToCanvasPos(rect,Input.mousePosition);
            float distancePin = Vector3.Distance(screenPos,Vector2.zero);
            m_dir = (screenPos - Vector2.zero).normalized;
            if (distancePin > m_distance)
                distancePin = m_distance;
            
            pin.anchoredPosition = m_dir * distancePin;
        }
        
        public Vector2 ScreenPointToCanvasPos(RectTransform rectIn,Vector2 pos)
        {
            Vector2 localpoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectIn, pos, canvas.worldCamera, out localpoint);
            return localpoint;
        }

        public Vector2 GetDirection()
        {
            return m_dir;
        }
    }
}


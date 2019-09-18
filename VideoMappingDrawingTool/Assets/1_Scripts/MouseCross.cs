using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawingTool
{
    public class MouseCross : MonoBehaviour
    {
        
        [SerializeField] private Mode mode = default;
        [SerializeField] private int size = 3;

        [SerializeField] private Camera cam = default;
        [SerializeField] private RectTransform rectTransform = default;

        // Start is called before the first frame update
        void Start()
        {
            Resize();
        }

        // Update is called once per frame
        void Update()
        {
            FollowMouse();
        }

        private void Resize()
        {
            if(mode == Mode.Horizontal) rectTransform.sizeDelta = new Vector2(Screen.width, size);
            else rectTransform.sizeDelta = new Vector2(size, Screen.height);
        }

        private void FollowMouse()
        {
            if (mode == Mode.Horizontal) rectTransform.anchoredPosition = new Vector2(0, Input.mousePosition.y);
            else rectTransform.anchoredPosition = new Vector2(Input.mousePosition.x, 0);
        }

        public enum Mode { Vertical, Horizontal};
    }
}
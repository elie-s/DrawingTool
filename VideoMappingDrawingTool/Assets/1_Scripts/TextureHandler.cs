using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DrawingTool
{
    public class TextureHandler : MonoBehaviour
    {
        [SerializeField] private RawImage image = default;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private int brushSize = 1;

        private Texture2D texture;

        // Start is called before the first frame update
        void Awake()
        {
            SetTexture();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0)) Draw(color);
        }

        private void Draw(Color _color)
        {
            int x = (int)Input.mousePosition.x;
            int y = (int)Input.mousePosition.y;

            for (int i = x - brushSize; i < x+brushSize; i++)
            {
                for (int j = y-brushSize; j < y+brushSize; j++)
                {
                    texture.SetPixel(i, j, _color);
                }
            }

            
            texture.Apply();
            image.texture = texture;
        }

        private void BrushSize()
        {

        }

        private void SetTexture()
        {
            image.texture = new Texture2D(Screen.width, Screen.height);
            texture = image.texture as Texture2D;
            for (int x = 0; x < Screen.width; x++)
            {
                for (int y = 0; y < Screen.height; y++)
                {
                    texture.SetPixel(x, y, Color.black);

                }
            }
            texture.Apply();
            image.texture = texture;
        }
    }
}
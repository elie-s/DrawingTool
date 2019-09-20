using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace DrawingTool
{
    public class TextureHandler : MonoBehaviour
    {
        [SerializeField] private GameObject savingText = default;
        [SerializeField] private RectTransform pointer = default;
        [SerializeField] private RawImage image = default;
        [SerializeField] private Color color = Color.white;
        [SerializeField] private int brushSize = 1;
        [SerializeField] private Mode mode = default;

        private Texture2D texture;
        private Texture2D lastModif;
        private bool sequence = false;

        // Start is called before the first frame update
        void Awake()
        {
            SetTexture();
        }

        // Update is called once per frame
        void Update()
        {
            BrushSize();
            Pointer();

            if (Input.GetMouseButton(0)) Draw(color);
            else if (Input.GetMouseButton(1)) Draw(Color.black);

            if (sequence && (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)))
            {
                Debug.Log("end sequence");
                sequence = false;
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                if(Input.GetKeyDown(KeyCode.S)) Save();
                if(Input.GetKeyDown(KeyCode.Z)) Undo();
            }
        }

        private void Draw(Color _color)
        {
            if (!sequence)
            {
                lastModif.SetPixels(texture.GetPixels());
                lastModif.Apply();
                sequence = true;
            }

            switch (mode)
            {
                case Mode.Circle:
                    DrawCircle(_color);
                    break;
                case Mode.Square:
                    DrawSquare(_color);
                    break;
                default:
                    break;
            }
        }

        private void DrawSquare(Color _color)
        {
            int x = (int)Input.mousePosition.x;
            int y = (int)Input.mousePosition.y;

            for (int i = x - brushSize; i < x + brushSize; i++)
            {
                for (int j = y - brushSize; j < y + brushSize; j++)
                {
                    texture.SetPixel(i, j, _color);
                }
            }


            texture.Apply();
        }

        private void DrawCircle(Color _color)
        {
            int x = 0;
            int y = brushSize;
            int m = 5 - (4 * y);

            int mX = (int)Input.mousePosition.x;
            int mY = (int)Input.mousePosition.y;

            while (x <= y)
            {
                texture.SetPixel(x + mX, y + mY, _color);
                texture.SetPixel(y + mX, x + mY, _color);
                texture.SetPixel(-x + mX, y + mY, _color);
                texture.SetPixel(-y + mX, x + mY, _color);
                texture.SetPixel(x + mX, -y + mY, _color);
                texture.SetPixel(y + mX, -x + mY, _color);
                texture.SetPixel(-x + mX, -y + mY, _color);
                texture.SetPixel(-y + mX, -x + mY, _color);

                if (m > 0)
                {
                    y--;
                    m -= 8 * y;
                }

                x++;
                m += 8 * x + 4;
            }

            texture.Apply();
        }

        private void Pointer()
        {
            pointer.sizeDelta = Vector2.one * brushSize*2;
            pointer.anchoredPosition = new Vector2(Input.mousePosition.x-brushSize, Input.mousePosition.y- brushSize);
        }

        private void BrushSize()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1)) brushSize = 1;
            else if (Input.GetKeyDown(KeyCode.Keypad2)) brushSize = 3;
            else if (Input.GetKeyDown(KeyCode.Keypad3)) brushSize = 5;
            else if (Input.GetKeyDown(KeyCode.Keypad4)) brushSize = 7;
            else if (Input.GetKeyDown(KeyCode.Keypad5)) brushSize = 10;
            else if (Input.GetKeyDown(KeyCode.Keypad6)) brushSize = 15;
            else if (Input.GetKeyDown(KeyCode.Keypad7)) brushSize = 20;
            else if (Input.GetKeyDown(KeyCode.Keypad8)) brushSize = 30;
            else if (Input.GetKeyDown(KeyCode.Keypad9)) brushSize = 50;
        }

        private void SetTexture()
        {
            image.texture = new Texture2D(Screen.width, Screen.height);
            texture = image.texture as Texture2D;
            lastModif = texture;
            for (int x = 0; x < Screen.width; x++)
            {
                for (int y = 0; y < Screen.height; y++)
                {
                    texture.SetPixel(x, y, Color.black);

                }
            }
            texture.Apply();
        }

        private void Save()
        {
            byte[] bytes = texture.EncodeToPNG();
            int index = 0;

            while (File.Exists(Application.dataPath + "/../SavedMask_" + index + ".png"))
            {
                index++;
            }
            File.WriteAllBytes(Application.dataPath + "/../SavedMask_" + index + ".png", bytes);
            StartCoroutine(SavingText());
        }

        private void Undo()
        {
            Debug.Log("undo");
            texture.SetPixels(lastModif.GetPixels());
            texture.Apply();
            
        }

        private IEnumerator SavingText()
        {
            savingText.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            savingText.SetActive(false);
        }

        public enum Mode { Circle, Square}
    }
}
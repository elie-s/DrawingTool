using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DrawingTool
{
    public class QuitApp : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                StartCoroutine(QuitRoutine());
            }
        }

        private IEnumerator QuitRoutine()
        {
            yield return null;

            float time = 0.0f;

            while (time < 0.75f)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Debug.Log("Quitting the app.");
                    Application.Quit();
                }

                time += Time.deltaTime;

                yield return null;
            }
        }
    }
}
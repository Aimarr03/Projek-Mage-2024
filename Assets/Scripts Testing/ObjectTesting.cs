using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursorTesting
{
    public class ObjectTesting : MonoBehaviour, I_PlayerInterractable

    {
        public void OnClicked()
        {
            Debug.Log("Object CLicked");
        }

        public void OnEnterCursor()
        {
            Debug.Log("Enter Cursor");
        }

        public void OnExitCursor()
        {
            Debug.Log("Exit Cursor");
        }

        void Start()
        {

        }
        void Update()
        {

        }
    }
}

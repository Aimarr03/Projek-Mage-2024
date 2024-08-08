using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace CursorTesting
{
    public class CursorTesting : MonoBehaviour
    {
        private PlayerController playerController;
        private I_PlayerInterractable interractableObject;
        private void Awake()
        {
            playerController = new PlayerController();
            playerController.PlayerOne.Enable();
        }
        void Start()
        {

        }
        void Update()
        {
            Vector3 inputDirection = playerController.PlayerOne.MoveController.ReadValue<Vector2>();
            transform.position += inputDirection * Time.deltaTime * 5f;
            RaycastHit2D objectHit = Physics2D.Raycast(transform.position, Vector3.forward, 10f);
            if (objectHit.collider == null)
            {
                if(this.interractableObject != null)
                {
                    this.interractableObject?.OnExitCursor();
                    this.interractableObject = null;
                }
                return;
            }
            if(objectHit.collider.gameObject.TryGetComponent(out I_PlayerInterractable interractableObject))
            {
                if(this.interractableObject != interractableObject)
                {
                    this.interractableObject?.OnExitCursor();
                    this.interractableObject = interractableObject;
                    this.interractableObject?.OnEnterCursor();
                }
                else
                {

                }
            }
        }
    }

}

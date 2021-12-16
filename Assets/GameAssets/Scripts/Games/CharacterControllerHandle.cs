using System.Collections;
using System.Collections.Generic;
using ECM2.Characters;
using ECM2.Common;
using KOLS;
using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterControllerHandle : AgentCharacter
{
    public string userId;
    public bool isPathFollowing;
    public GameObject pointCameraFollow;
    private JoystickHandle m_joystickHandle;
    

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();
        m_joystickHandle = JoystickHandle.Instance;
        PlayerClientManager.Instance.AddPlayer(this);
    }

    public override bool IsPathFollowing()
    {
        if (!isPathFollowing)
            return false;
        return base.IsPathFollowing();
    }

    public void SetCamera(Camera cameraIn)
    {
        _camera = cameraIn;
    }

    protected override void HandleInput()
    {
        if(!PhotonNetwork.LocalPlayer.IsLocal)
            return;
        
        //block when click UI
        if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButton(0) && IsPathFollowing())
        {
            //Vector2 mousePosition = GetMousePosition();
            Vector2 mousePosition = Input.mousePosition;
            Ray ray = camera.ScreenPointToRay(mousePosition);
            LayerMask groundMask = characterMovement.groundMask;
            QueryTriggerInteraction queryTriggerInteraction = characterMovement.collideWithTriggers
                ? QueryTriggerInteraction.Collide
                : QueryTriggerInteraction.Ignore;

            if (Physics.Raycast(ray, out RaycastHit hitResult, Mathf.Infinity, groundMask, queryTriggerInteraction))
                MoveToLocation(hitResult.point);
        }
        else
        {
            // Default movement input. Allow to controll the agent with keyboard or controller too
            //base.HandleInput();
            if (m_joystickHandle == null)
            {
                base.HandleInput();
            }
            else
            {
                InputWithJoystick();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    StopJumping();
                }
            }
        }

        void InputWithJoystick()
        {
            if(m_joystickHandle == null)
                return;
            
            if (cameraTransform == null)
            {
                // If Camera is not assigned, add movement input relative to world axis

                Vector3 movementDirection = Vector3.zero;

                movementDirection += Vector3.right * m_joystickHandle.GetDirection().x;
                movementDirection += Vector3.forward * m_joystickHandle.GetDirection().y;

                SetMovementDirection(movementDirection);
            }
            else
            {
                // If Camera is assigned, add input movement relative to camera look direction

                Vector3 movementDirection = Vector3.zero;

                movementDirection += Vector3.right * m_joystickHandle.GetDirection().x;
                movementDirection += Vector3.forward * m_joystickHandle.GetDirection().y;
                
                movementDirection = movementDirection.relativeTo(cameraTransform);

                SetMovementDirection(movementDirection);
            }
        }
    }
}

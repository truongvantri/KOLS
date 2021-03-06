using System.Collections;
using System.Collections.Generic;
using ECM2.Characters;
using UnityEngine;

public class CharacterControllerHandle : AgentCharacter
{
    protected override void HandleInput()
    {
        // Should handle input here ?
        // Movement (click-to-move)

        if (Input.GetMouseButton(0))
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
        else if (!IsPathFollowing())
        {
            // Default movement input. Allow to controll the agent with keyboard or controller too

            base.HandleInput();
        }
    }
}

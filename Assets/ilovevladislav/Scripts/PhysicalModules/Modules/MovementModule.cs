using UnityEngine;

namespace PlatformerController2D.Runtime.Scripts.PhysicalModules.Modules
{
    public class MovementModule 
    {
        public enum InterpolationType
        {
            MoveTowards,
            Lerp
        }
    
        private Vector2 _targetVelocity;

        public MovementModule() { }
    
        public Vector2 HandleMovement(Vector2 moveVelocity, Vector2 moveDirection, float speed, float acceleration, float deceleration, InterpolationType interpolationType = InterpolationType.MoveTowards)
        {
            _targetVelocity = moveDirection != Vector2.zero
                ? new Vector2(moveDirection.x, 0f) * speed
                : Vector2.zero;
        
            float smoothFactor = moveDirection != Vector2.zero
                ? acceleration
                : deceleration;
            
            switch (interpolationType)
            {
                case InterpolationType.MoveTowards:
                    moveVelocity.x = ApplyMoveTowards(moveVelocity, smoothFactor);
                    break;
                case InterpolationType.Lerp:
                    moveVelocity.x = ApplyLerp(moveVelocity, smoothFactor);
                    break;
            }
    
            return moveVelocity;
        }

        private float ApplyMoveTowards(Vector2 currentVelocity, float smoothFactor)
        {
            var moveVelocityX = Mathf.MoveTowards(
                currentVelocity.x,
                _targetVelocity.x,
                smoothFactor * Time.fixedDeltaTime
            );

            return moveVelocityX;
        }
    
        private float ApplyLerp(Vector2 currentVelocity, float smoothFactor)
        {
            var moveVelocityX = Vector2.Lerp(currentVelocity, _targetVelocity, smoothFactor * Time.fixedDeltaTime).x;
        
            // if (Mathf.Abs(moveVelocityX) < 0.1f) 
            // {
            //     moveVelocityX = 0f;
            // }

            return moveVelocityX;
        }
    }
}
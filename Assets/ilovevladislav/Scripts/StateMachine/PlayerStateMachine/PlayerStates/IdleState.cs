using UnityEngine;

namespace PlatformerController2D.Runtime.Scripts.StateMachine.PlayerStateMachine.PlayerState
{
	public class IdleState : BaseState
	{
		public IdleState(PlayerStateContext context) :
			base(context) { }

		public override void OnEnter()
		{
			Debug.Log("IdleState");
            Debug.Log("idle state axe check outer" + StaticVars.instance.AxeFlag);
            if (StaticVars.instance.AxeFlag)
            {
                animationController.PlayAnimation("Idle");
				Debug.Log("idle state axe check" + StaticVars.instance.AxeFlag);
            }
            else
            {
                animationController.PlayAnimation("Idle 1");
            }

            
		}

		public override void Update()
		{
			turnChecker.TurnCheck(inputReader.GetMoveDirection());
		}

		public override void FixedUpdate()
		{
			var moveVelocity = new Vector2(0f, playerControllerStats.GroundGravity);
			physicsHandler2D.AddVelocity(moveVelocity);
		}	
	}
}
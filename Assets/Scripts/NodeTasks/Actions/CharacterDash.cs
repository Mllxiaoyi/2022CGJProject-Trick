using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Actions{

	[Category("Movement")]
	public class CharacterDash : ActionTask<CharacterController2D>{

		public float dashDistance;
		public float dashTime;

        public int _dir=-1;
		public int faceToAfter = 1;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit(){
			return null;
		}


		//Called once per frame while the action is active.
		protected override void OnUpdate(){
			if (elapsedTime > dashTime)
			{
				EndAction(true);
				return;
			}
            if (agent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Dash"))
            {
				agent.MoveTowards(dashDistance / dashTime, _dir);
			}			

		}

		//Called when the task is disabled.
		protected override void OnStop(){
			agent.FaceTo(faceToAfter);
			agent.Stop();
		}

		//Called when the task is paused.
		protected override void OnPause(){
			
		}
	}
}
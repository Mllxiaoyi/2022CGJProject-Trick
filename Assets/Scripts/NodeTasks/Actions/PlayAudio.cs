using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;
using Game;

namespace NodeCanvas.Tasks.Actions
{
	public class PlayAudio : ActionTask
	{
		[RequiredField]
		public Game.AudioSO audioSO;

		//Use for initialization. This is called only once in the lifetime of the task.
		//Return null if init was successfull. Return an error string otherwise
		protected override string OnInit()
		{
			return null;
		}

        protected override void OnExecute()
        {
            base.OnExecute();
			AudioManager.Instance.PlayAudio(audioSO);
			EndAction(true);

		}
        //Called when the task is paused.
        protected override void OnPause()
		{

		}
	}
}
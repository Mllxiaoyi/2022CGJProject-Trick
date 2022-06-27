using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace NodeCanvas.Tasks.Conditions{

	[Category("Game")]
	public class CheckHorizontalDistance : ConditionTask<Transform>{

		public BBParameter<Transform> target;
		public CompareMethod checkType = CompareMethod.LessThan;
		public BBParameter<float> distance=1;

		[SliderField(0, 0.1f)]
		public float floatingPoint = 0.05f;
		protected override string OnInit(){
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable(){
			
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable(){
			
		}

		protected override bool OnCheck(){
			return OperationTools.Compare(Mathf.Abs(target.value.position.x-agent.position.x), distance.value, checkType, floatingPoint);
		}
	}
}
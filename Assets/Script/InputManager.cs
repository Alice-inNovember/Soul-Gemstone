using UnityEngine;
using Util.EventSystem;
using Util.SingletonSystem;

namespace Script
{
	public class InputManager : MonoBehaviourSingleton<InputManager>
	{
		private Vector3 _startPos;

		private void Update()
		{
			MouseInput();
		}

		private void MouseInput()
		{
			var mousePos = Input.mousePosition;

			//마우스 클릭시 시작 포지션 초기화
			if (Input.GetMouseButtonDown(0))
				_startPos = mousePos;

			//이동거리
			var movedDistance = (mousePos - _startPos).magnitude;

			//마우스 버튼 업 X || 이동거리가 100 미만 || x보다 y의 움직임이 크면 리턴
			if (!Input.GetMouseButtonUp(0) || movedDistance < 100 ||
			    Mathf.Abs(mousePos.x - _startPos.x) < Mathf.Abs(mousePos.y - _startPos.y))
				return;

			//마우스 버튼 업, 이동거리가 100 이상, x의 움직임이 y보다 크면 인터렉션 호출
			var interactionType = mousePos.x > _startPos.x
				? EInteractionType.RightSwipe
				: EInteractionType.LeftSwipe;
			EventManager.Instance.PostNotification(EEventType.ScreenInteraction, this, interactionType);
		}
	}

	public enum EInteractionType
	{
		LeftSwipe,
		RightSwipe
	}
}
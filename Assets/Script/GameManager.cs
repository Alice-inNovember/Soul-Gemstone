using Util.EventSystem;
using Util.SingletonSystem;

namespace Script
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public EPlayerStatus currentStatus;

        private void Start()
        {
            currentStatus = EPlayerStatus.Book;
            InputManager.Instance.eventType = EEventType.ScreenInterection;
        }
    }

    public enum EPlayerStatus
    {
        Book,
        Diary
    }
}
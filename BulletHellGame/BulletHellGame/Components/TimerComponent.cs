using System.Timers;

namespace BulletHellGame.Components
{
    public class TimerComponent : IComponent
    {
        private Timer _timer;
        public bool TimeElapsed = false;

        public TimerComponent(int duration)
        {
            _timer = new Timer(duration);
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimeElapsed = true;
        }
    }
}

namespace BulletHellGame.Logic.Commands
{
    public class CommandManager
    {
        private Queue<ICommand> _commandQueue = new Queue<ICommand>();

        public void QueueCommand(ICommand command)
        {
            _commandQueue.Enqueue(command); // Add the command to the queue
        }

        public void ExecuteCommands()
        {
            while (_commandQueue.Count > 0)
            {
                var command = _commandQueue.Dequeue();
                command.Execute(); // Execute each command
            }
        }
    }

}

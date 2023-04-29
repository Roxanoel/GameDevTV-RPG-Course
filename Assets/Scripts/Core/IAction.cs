namespace RPG.Core
{
    public interface IAction
    {
        // All actions have to implement a cancel method
        void Cancel();
    }
}


using Windows.ApplicationModel.Background;

namespace Background
{
    public sealed class SyncTask : IBackgroundTask
    {
        public readonly string TaskName = "Pr0SyncTask";
        void IBackgroundTask.Run(IBackgroundTaskInstance taskInstance)
        {
            
        }
    }
}
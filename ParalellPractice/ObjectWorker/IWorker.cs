namespace ObjectWorker {
    internal interface IWorker {
        void ProcessJobAsync(object job);
    }
}
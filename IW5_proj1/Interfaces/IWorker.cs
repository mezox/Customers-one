using System;
namespace IW5_proj1
{
    public interface IWorker<T>
     where T : new()
    {
        T Load(out string exception);
        void Save(T group);
        void setOutputFile(string newFile);
    }
}

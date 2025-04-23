namespace TestClickerEcs
{
    public interface IEcsEventListener<T> where T : struct
    {
        void OnEvent(ref T evt);
    }
}


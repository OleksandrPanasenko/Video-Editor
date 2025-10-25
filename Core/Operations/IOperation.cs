namespace VideoEditor.Core
{
    public interface IOperation
    {
        public string Name { get; }
        public void Apply();
        void Undo();
        
    }
}
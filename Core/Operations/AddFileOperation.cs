namespace VideoEditor.Core
{
    public class AddFileOperation : IOperation
    {
        public string Name => new string("AddFile");
        private readonly Project Project;
        private readonly string FilePath;
        public AddFileOperation(Project project, string filePath)
        {
            Project = project;
            FilePath = filePath;
        }
        public void Apply()
        {
            Project.AddMediaFile(FilePath);
        }
        public void Undo()
        {
            Project.RemoveMediaFile(FilePath);
            //What if file doesn't exist?
        }
    }
}
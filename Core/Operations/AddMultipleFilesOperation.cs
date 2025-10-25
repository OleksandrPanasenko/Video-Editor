namespace VideoEditor.Core
{
    public class AddMultipleFilesOperation : IOperation
    {
        public string Name => new string("AddFile");
        private readonly Project Project;
        private readonly List <string> FilePathList;
        public AddMultipleFilesOperation(Project project, string[] filePathList)
        {
            Project = project;
            FilePathList=new List<string>();
            foreach (string filePath in filePathList)
            {
                if (File.Exists(filePath)& !Project.MediaFiles.Contains(filePath))
                {
                    FilePathList.Add(filePath);
                }
            }
        }
        public void Apply()
        {
            foreach (string filePath in FilePathList)
            {
                Project.AddMediaFile(filePath);
            }
        }
        public void Undo()
        {
            foreach (string filePath in FilePathList)
            {
                Project.RemoveMediaFile(filePath);
            }
            //What if file doesn't exist?
        }
    }
}
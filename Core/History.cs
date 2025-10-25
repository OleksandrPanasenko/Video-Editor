using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//TODO: Add Comments
//TODO: Change Stack to Deque for better performance
//TODO: Add way to change maxSteps
namespace VideoEditor.Core
{
    public class History
    {
        private readonly Stack<IOperation> undoStack = new Stack<IOperation>();
        private readonly Stack<IOperation> redoStack = new Stack<IOperation>();
        private readonly int maxSteps;

        public History(int maxSteps)
        {
            this.maxSteps = maxSteps;
        }

        public void Execute(IOperation operation)
        {
            operation.Apply();
            undoStack.Push(operation);
            redoStack.Clear();
            if (undoStack.Count > maxSteps)
            {
                // Remove the oldest operation to maintain the max steps limit
                var tempStack = new Stack<IOperation>(undoStack.Reverse().Skip(1));
                undoStack.Clear();
                foreach (var op in tempStack.Reverse())
                {
                    undoStack.Push(op);
                }
            }
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                var operation = undoStack.Pop();
                operation.Undo();
                redoStack.Push(operation);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                var operation = redoStack.Pop();
                operation.Apply();
                undoStack.Push(operation);
            }
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
    }
}

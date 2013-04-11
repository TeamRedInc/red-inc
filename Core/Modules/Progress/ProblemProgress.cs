using core.Modules.Problem;

namespace core.Modules.Progress
{
    public class ProblemProgress : ProblemData
    {
        private int numCorrect;
        private double avgAttempts;

        public ProblemProgress() : base() { }

        public ProblemProgress(int id) : base(id) { }

        public int NumCorrect
        {
            get { return numCorrect; }
            set { numCorrect = value; }
        }

        public bool IsSolved
        {
            get { return numCorrect == 1; }
        }

        public double AvgAttempts
        {
            get { return avgAttempts; }
            set { avgAttempts = value; }
        }
    }
}

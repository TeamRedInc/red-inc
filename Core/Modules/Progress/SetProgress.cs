using core.Modules.ProblemSet;

namespace core.Modules.Progress
{
    public class SetProgress : ProblemSetData
    {
        private int numCorrect;
        private double avgAttempts;

        public SetProgress() : base() { }

        public SetProgress(int id) : base(id) { }

        public int NumCorrect
        {
            get { return numCorrect; }
            set { numCorrect = value; }
        }

        public double AvgAttempts
        {
            get { return avgAttempts; }
            set { avgAttempts = value; }
        }
    }
}

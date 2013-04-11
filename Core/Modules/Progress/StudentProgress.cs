using core.Modules.User;

namespace core.Modules.Progress
{
    public class StudentProgress : UserData
    {
        private int numCorrect;
        private double avgAttempts;

        public StudentProgress() : base() { }

        public StudentProgress(int id) : base(id) { }

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

using System.Windows.Forms;

namespace VacuumCleaner_SRA
{
    internal class EnvironmentVacuum
    {
        private char loc;
        private int stat;
        private PictureBox picBox;
        public char Loc
        {
            get { return loc; }
            set { loc = value; }
        }
        public int Stat
        {
            get { return stat; }
            set { stat = value; }
        }
        public PictureBox PicBox
        {
            get { return picBox; }
            set { picBox = value; }
        }
    }
}
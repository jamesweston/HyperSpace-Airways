using System.Threading;
using System.Windows.Forms;

namespace HyperSpace_Airways
{
    public class Semaphore
    {
        public Semaphore(PictureBox init)
        {
            pb = init;
        }
        private int count = 0; //Store the semaphore count
        private PictureBox pb;

        public void Wait() //semaphore wait method
        {
            lock (this) //semaphore lock
            {
                if (count == 0) // if count is 0 call monitor wait
                    Monitor.Wait(this);
                count = 0; // set count to 0
                pb.Image = Properties.Resources.Red_light;
            }
        }

        public void Signal() //semaphore signal method
        {
            lock (this) //semaphore lock
            {
                count = 1; //set count to want
                pb.Image = Properties.Resources.Green_light;
                Monitor.Pulse(this); // pulse semaphore
            }
        }

        public void Start()
        {
        }

    }// end class Semaphore
}

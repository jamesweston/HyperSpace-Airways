using System.Threading;

namespace HyperSpace_Airways
{
    public class XSemaphore
    {
        private int count;

        public XSemaphore(int init)
        {
            count = init;
        }
        public void Wait()
        {
            lock (this)
            {
                if (count == 0)
                    Monitor.Wait(this);
                count--;
            }
        }

        public void Signal()
        {
            lock (this)
            {
                count++;
                Monitor.Pulse(this);
            }
        }

        public int Peak()
        {
            lock (this)
            {
                return count;
            }
        }

        public void Start()
        {
        }

    }// end class Semaphore
}

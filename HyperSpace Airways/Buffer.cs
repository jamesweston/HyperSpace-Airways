using System.Drawing;
using System.Threading;

namespace HyperSpace_Airways
{
    public class Buffer
    {
        private Color planeColor; //Stores plane colour
        private int dest; //Stores plane destination
        private bool empty = true; //If buffer is empty

        public void Read(ref Color planeColor, ref int dest) //Read method with paramiters
        {
            lock (this) //Lock to ensure no conncurrent access
            {
                // Check whether the buffer is empty.
                if (empty) //Wait buffer is empty
                    Monitor.Wait(this); //Force thread to wait
                empty = true; //Set buffer as empty
                planeColor = this.planeColor; //Copy plane colour from buffer
                dest = this.dest; //Copy plane desination from buffer
                Monitor.Pulse(this); //Pulse buffer
            }
        }

        public void Write(Color planeColor, int dest)//Write method with paramiters
        {
            lock (this)//Lock to ensure no conncurrent access
            {
                // Check whether the buffer is full.
                if (!empty)//Wait buffer is full
                    Monitor.Wait(this);
                empty = false; //Set buffer as full
                this.planeColor = planeColor; //Copy plane colour to buffer
                this.dest = dest; //Copy plane desination to buffer
                Monitor.Pulse(this);//Pulse buffer
            }
        }

        public void Start()
        {
        }

    }// end class Buffer
}

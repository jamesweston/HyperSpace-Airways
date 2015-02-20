using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HyperSpace_Airways
{
    public class WaitPanelThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private bool westEast;
        private Color colour;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphore, semaphoreAlt, sm2;
        private Buffer buffer, bufferAlt, bf2;
        private int dest;
        private int destact;
        private Image img = Properties.Resources.pxl;

        public WaitPanelThread(Point origin,
                           int delay,
                           bool westEast,
                           Panel panel,
                           Color colour,
                           Semaphore semaphore,
                           Semaphore semaphoreAlt,
                            Semaphore sm2,
                           Buffer buffer,
                           Buffer bufferAlt,
                            Buffer bf2, int destact)
        {
            this.origin = origin;
            this.delay = delay;
            this.westEast = westEast;
            this.panel = panel;
            this.colour = colour;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = westEast ? +10 : -10;
            this.yDelta = 0;
            this.semaphore = semaphore;
            this.semaphoreAlt = semaphoreAlt;
            this.sm2 = sm2;
            this.buffer = buffer;
            this.bufferAlt = bufferAlt;
            this.bf2 = bf2;
            this.destact = destact;

        }

        public void Start()
        {

            //Thread.Sleep(delay);
            this.colour = Color.White;
            lock (Properties.Resources.pxl)
            {
                img = Properties.Resources.pxl;
            }
            while (General.MasterSwitch == true)
            {
                semaphore.Signal();
                this.zeroPlane();

                buffer.Read(ref this.colour, ref this.dest);
                lock (Properties.Resources.aeroplane)
                {
                    img = Properties.Resources.aeroplane;
                }
                for (int i = 1; i <= 24; i++)
                {

                    panel.Invalidate();
                    this.movePlane(xDelta, yDelta);
                    Thread.Sleep(delay);

                }

                if ((dest == destact) & (dest != 4))
                {
                    sm2.Wait();
                    bf2.Write(this.colour, this.dest);
                    lock (Properties.Resources.pxl)
                    {
                        img = Properties.Resources.pxl;
                    }
                    this.colour = Color.White;
                    panel.Invalidate();
                }
                else
                {
                    semaphoreAlt.Wait();
                    bufferAlt.Write(this.colour, this.dest);
                    lock (Properties.Resources.pxl)
                    {
                        img = Properties.Resources.pxl;
                    }
                    this.colour = Color.White;
                    panel.Invalidate();
                }
                    

            }
            //this.colour = Color.Gray; MessageBox.Show("Test");
            //panel.Invalidate();
        }

        private void zeroPlane()
        {
            plane.X = origin.X;
            plane.Y = origin.Y;
        }

        private void movePlane(int xDelta, int yDelta)
        {
            plane.X += xDelta; plane.Y += yDelta;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(colour);
            Image planimg = img;
            g.FillRectangle(brush, plane.X, plane.Y, 25, 25);
            lock (Properties.Resources.aeroplane)
            {
                g.DrawImage(planimg, plane.X, plane.Y, 25, 25);
            }
            //planimg.Dispose();
            brush.Dispose();    //  Dispose graphics resources. 
            g.Dispose();        // 
        }
    }// end class WaitPanelThread
}

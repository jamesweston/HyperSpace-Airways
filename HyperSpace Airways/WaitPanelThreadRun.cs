using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HyperSpace_Airways
{
    public class WaitPanelThreadRun
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private bool westEast;
        private Color colour;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphore, semaphoreAlt;
        private Buffer buffer;
        private Buffer bufferAlt;
        private int dest;
        private Image img = Properties.Resources.pxl;


        public WaitPanelThreadRun(Point origin,
                           int delay,
                           bool westEast,
                           Panel panel,
                           Color colour,
                           Semaphore semaphore,
                           Semaphore semaphoreAlt,
                           Buffer buffer,
                           Buffer bufferAlt)
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
            this.buffer = buffer;
            this.bufferAlt = bufferAlt;

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
                    img.RotateFlip(RotateFlipType.Rotate180FlipY);
                }
                for (int i = 1; i <= 80; i++)
                {

                    panel.Invalidate();
                    if (this.dest == 4)
                    {
                        this.movePlane(xDelta + i, yDelta);
                    }
                    else
                    {
                        this.movePlane(xDelta, yDelta);
                    }
                    Thread.Sleep(delay);

                }
                if (this.dest == 4)
                {

                }
                else if (this.dest < 4)
                {
                    semaphoreAlt.Wait();
                    bufferAlt.Write(this.colour, this.dest);
                }
                this.colour = Color.White;
                lock (Properties.Resources.pxl)
                {
                    img = Properties.Resources.pxl;
                }
                panel.Invalidate();

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
            plane.X -= xDelta ; plane.Y -= yDelta;
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

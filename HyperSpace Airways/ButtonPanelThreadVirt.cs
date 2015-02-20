using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HyperSpace_Airways
{
    public class ButtonPanelThreadVirt
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
        private Buffer buffer, bufferAlt;
        private Button btn;
        private int dest = 4;
        private bool locked = true;
        private Image img = Properties.Resources.pxl;
        private Graphics g;
        private SolidBrush brush;
        private Image planimg;
       
   


        public ButtonPanelThreadVirt(Point origin,
                                 int delay,
                                 bool westEast,
                                 Panel panel,
                                 Color colour,
                                 Semaphore semaphore,
                                   Semaphore semaphoreAlt,
                                 Buffer buffer,
                                 Buffer bufferAlt,
                                 Button btn)
        {
            this.origin = origin;
            this.delay = delay;
            this.westEast = westEast;
            this.panel = panel;
            this.colour = colour;
            this.plane = origin;
            this.panel.Paint += new PaintEventHandler(this.panel_Paint);
            this.xDelta = 0;
            this.yDelta = westEast ? +10 : -10;
            this.semaphore = semaphore;
            this.buffer = buffer;
            this.semaphoreAlt = semaphoreAlt;
            this.bufferAlt = bufferAlt;
            this.btn = btn;
            this.btn.Click += new System.
                                  EventHandler(this.btn_Click);


        }

        private void btn_Click(object sender,
                               System.EventArgs e)
        {
            ButtonAction();
        }

        public void Start()
        {
            
            Color signal = Color.White;
            //img.RotateFlip(RotateFlipType.Rotate90FlipX);
            Thread.Sleep(delay);
            while (General.MasterSwitch == true)
            {
                //if (Count == 1)
                //{

                    this.zeroPlane();
                    lock (Properties.Resources.aeroplane)
                    {
                        img = Properties.Resources.aeroplane;
                        img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    }
                    panel.Invalidate();
                    lock (this)
                    {
                        while (locked)
                        {
                            Monitor.Wait(this);
                        }
                    }
                    for (int i = 1; i <= 19; i++)
                    {
                        this.movePlane(xDelta, yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }
                    semaphore.Wait();
                    buffer.Write(this.colour, 4);
                    lock (Properties.Resources.pxl)
                    {
                        img = Properties.Resources.pxl;
                    }
                    this.colour = Color.White;
                    panel.Invalidate();
                    ButtonAction();
                //}
                //else
                //{
                    semaphoreAlt.Signal();
                    bufferAlt.Read(ref this.colour, ref this.dest);
                    lock (Properties.Resources.aeroplane)
                    {
                        img = Properties.Resources.aeroplane;
                        img.RotateFlip(RotateFlipType.Rotate90FlipY);
                    }
                    for (int i = 1; i <= 19; i++)
                    {
                        this.movePlaneRev(xDelta, yDelta);
                        Thread.Sleep(delay);
                        panel.Invalidate();
                    }
                    lock (this)
                    {
                        while (locked)
                        {
                            Monitor.Wait(this);
                        }
                    }
                //}
                
            }
            this.colour = Color.Gray;
            panel.Invalidate();
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

        private void movePlaneRev(int xDelta, int yDelta)
        {
            plane.X -= xDelta; plane.Y -= yDelta;
        }

        private void panel_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            brush = new SolidBrush(colour);
            planimg = img;
            g.FillRectangle(brush, plane.X, plane.Y, 25, 25);
            lock (Properties.Resources.aeroplane)
            {
                g.DrawImage(planimg, plane.X, plane.Y, 25, 25);
            }
       
            brush.Dispose();    //  Dispose graphics resources. 
            g.Dispose();        // 
            //planimg.Dispose();
        }

        private void ButtonAction()
        {
            locked = !locked;
            this.btn.BackColor = locked ? Color.Pink : Color.LightGreen;
            lock (this)
            {
                if (!locked)
                    Monitor.Pulse(this);
            }
        }

    }// end class ButtonPanelThread
}

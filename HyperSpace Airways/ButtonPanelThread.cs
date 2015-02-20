using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HyperSpace_Airways
{
    public class ButtonPanelThread
    {
        private Point origin;
        private int delay;
        private Panel panel;
        private bool westEast;
        private Color colour;
        private Point plane;
        private int xDelta;
        private int yDelta;
        private Semaphore semaphore;
        private Buffer buffer;
        private Button btn;
        private bool locked = true;
        private Image img = Properties.Resources.aeroplane;
        private RadioButton[] cbarray = new RadioButton[4];
        private int currentrb = 0;

        public ButtonPanelThread(Point origin, //ButtonPanelThread Constructer
                                 int delay,
                                 bool westEast,
                                 Panel panel,
                                 Color colour,
                                 Semaphore semaphore,
                                 Buffer buffer,
                                 Button btn,
                                 RadioButton[] rba)
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
            this.buffer = buffer;
            this.btn = btn;
            this.cbarray = rba;
            this.btn.Click += new System.
                                  EventHandler(this.btn_Click);
            this.cbarray[0].CheckedChanged += new System.EventHandler(this.rb_CheckedChanged0); //Radio Button Event Handler
            this.cbarray[1].CheckedChanged += new System.EventHandler(this.rb_CheckedChanged1); //Radio Button Event Handler
            this.cbarray[2].CheckedChanged += new System.EventHandler(this.rb_CheckedChanged2); //Radio Button Event Handler
            this.cbarray[3].CheckedChanged += new System.EventHandler(this.rb_CheckedChanged3); //Radio Button Event Handler
           


        }

        private void btn_Click(object sender,
                               System.EventArgs e)
        {
            ButtonAction(); //Button action to handle button click and auto stop
        }

        public void Start()
        {
            lock (Properties.Resources.aeroplane) // locl image resources
            {
                img.RotateFlip(RotateFlipType.Rotate180FlipY); //rotate plane image
            }
            Thread.Sleep(delay); // sleep 


            while (General.MasterSwitch == true)
            {
                this.zeroPlane();
                panel.Invalidate();
                lock (this)
                {
                    while (locked)
                    {
                        Monitor.Wait(this);
                    }
                }
                for (int i = 1; i <= 20; i++)
                {
                    this.movePlane(xDelta, yDelta);
                    Thread.Sleep(delay);
                    panel.Invalidate();
                }
                semaphore.Wait();
                //destsem.Signal();
                buffer.Write(this.colour, currentrb);
                ButtonAction();
               
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

        private void rb_CheckedChanged0(object sender, System.EventArgs e)
        {
            currentrb = 0;
            colour = Color.Orange;
        }

        private void rb_CheckedChanged1(object sender, System.EventArgs e)
        {
            currentrb = 1;
            colour = Color.Red;

        }

        private void rb_CheckedChanged2(object sender, System.EventArgs e)
        {
            currentrb = 2;
            colour = Color.Green;
        }

        private void rb_CheckedChanged3(object sender, System.EventArgs e)
        {
            currentrb = 3;
            colour = Color.Blue;
        }

    }// end class ButtonPanelThread
}

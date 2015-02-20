using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace HyperSpace_Airways
{
    public partial class Form1 : Form
    {
        private ButtonPanelThreadVirt p1, p2, p3;
        private WaitPanelThreadVirt p7, p10;
        private ButtonPanelThread p9;
        private WaitPanelThread p4,p5,p6;
        private WaitPanelThreadRun p8;
        private Thread thread1, thread2, thread3, thread4, thread5, thread6, thread7, thread8, thread9, thread10;
        private Semaphore semaphore, sm2, sm3, sm4, sm5, sm6, semaphoreA, semaphoreB, semaphoreC;
        private Buffer buffer,bufferB, bufferC, bufferD, bufferE, bufferF, buffer1, buffer2, buffer3;
        private RadioButton[] cbarray = new RadioButton[4];

        public Form1()
        {
            InitializeComponent();
            cbarray[0] = rb0;
            cbarray[1] = rb1;
            cbarray[2] = rb2;
            cbarray[3] = rb3;
            semaphore = new Semaphore(pb1);
            semaphoreA = new Semaphore(pb2);
            semaphoreB = new Semaphore(pb3);
            semaphoreC = new Semaphore(pb4);
            sm2 = new Semaphore(pb5);
            sm3 = new Semaphore(pb6);
            sm4 = new Semaphore(pb7);
            sm5 = new Semaphore(pb8);
            sm6 = new Semaphore(pb9);
            buffer = new Buffer();
            buffer1 = new Buffer();
            buffer2 = new Buffer();
            buffer3 = new Buffer();
            bufferB = new Buffer();
            bufferC = new Buffer();
            bufferD = new Buffer();
            bufferE = new Buffer();
            bufferF = new Buffer();


            p1 = new ButtonPanelThreadVirt(new Point(10, 40),
                                 120, true, pnl1,
                                 Color.Red,
                                 semaphore,
                                 semaphoreA,
                                 buffer,
                                 buffer1,
                                 btn1);

            p2 = new ButtonPanelThreadVirt(new Point(10, 40),
                     120, true, pnl2,
                     Color.Green,
                     sm2,
                     semaphoreB,
                     bufferB,
                     buffer2,
                     btn2);

            p3 = new ButtonPanelThreadVirt(new Point(10, 40),
                     120, true, pnl3,
                     Color.Blue,
                     sm3,
                     semaphoreC,
                     bufferC,
                     buffer3,
                     btn3);

            p4 = new WaitPanelThread(new Point(10, 10),
                     100, true, pnl4,
                     Color.White,
                     semaphore,
                     sm2,semaphoreB,
                     buffer,
                     bufferB, buffer2, 2);

            p5 = new WaitPanelThread(new Point(10, 10),
                    100, true, pnl5,
                    Color.White,
                    sm2,
                    sm3, semaphoreC,
                    bufferB,
                    bufferC, buffer3, 3);

            p6 = new WaitPanelThread(new Point(10, 10),
                    100, true, pnl6,
                    Color.White,
                    sm3,
                    sm4,null,
                    bufferC,
                    bufferD,null,4);

            p7 = new WaitPanelThreadVirt(new Point(10, 10),
                     100, true, pnl7,
                     Color.White,
                     sm4,
                     sm5,
                     null,
                     bufferD,
                     bufferE,
                     null);

            p9 = new ButtonPanelThread(new Point(200, 10),
             120, false, pnl9,
             Color.Orange,
             sm5,
             bufferE,
             btn4,cbarray);

            p8 = new WaitPanelThreadRun(new Point(920, 10),
                     100, true, pnl8,
                    Color.White,
                    sm5,
                    sm6,
                    bufferE,
                    bufferF);


            p10 = new WaitPanelThreadVirt(new Point(10, 200),
                      100, false, pnl10,
                    Color.White,
                    sm6,
                    semaphore,
                    semaphoreA,
                    bufferF,
                    buffer,
                    buffer1);

     
            thread1 = new Thread(new ThreadStart(p1.Start));
            thread2 = new Thread(new ThreadStart(p2.Start));
            thread3 = new Thread(new ThreadStart(p3.Start));
            thread4 = new Thread(new ThreadStart(p4.Start));
            thread5 = new Thread(new ThreadStart(p5.Start));
            thread6 = new Thread(new ThreadStart(p6.Start));
            thread7 = new Thread(new ThreadStart(p7.Start));
            thread8 = new Thread(new ThreadStart(p8.Start));
            thread9 = new Thread(new ThreadStart(p9.Start));
            thread10 = new Thread(new ThreadStart(p10.Start));
            
            thread1.Start();
            thread2.Start();
            thread3.Start();
            thread4.Start();
            thread5.Start();
            thread6.Start();
            thread7.Start();
            thread8.Start();
            thread9.Start();
            thread10.Start();

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            thread10.Abort();
            thread9.Abort();
            thread8.Abort();
            thread7.Abort();
            thread6.Abort();
            thread5.Abort();
            thread4.Abort();
            thread3.Abort();
            thread2.Abort();
            thread1.Abort();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Testers
{
    public delegate void textStatusUpdate(object t, string message, string prefix);

    class Tester
    {
        public static event textStatusUpdate OnTextStatusUpdate;

        /*
            1 : writing program
            2 : testing program
            3 : sleeping
            4 : fixing program
            5 : testing fixed program
        */
        public int status { private set; get; } = 1;

        public string name { private set; get; } = "Unnamed";

        public Prog ownProgram                   = null;

        public Prog needToTest                   = null;

        public int index;

        private ManualResetEvent are;

        private int sleepDuration;

        private static string[] statusPrefix = { "[WRITING]", "[TESTING]", "[SLEEPING]", "[FIXING]", "[TESTING]" };

        public Tester(string name, int index)
        {
            this.name  = name;
            this.index = index;
            this.are = new ManualResetEvent(false);

            this.sleepDuration = Util.randInt(7000, 9000);
            __changeStatus(1);
        }

        public void MakeOwnProgram()
        {
            lock(this)
            {
                __changeTextStatus("Making own program");
                Thread.Sleep(Util.randInt(4000, 6000));
                ownProgram = new Prog() { writer = this };
                ProgPool.add(ownProgram);
                __changeTextStatus("Finished own program");

                __changeStatus(2);
            }   
        }

        public void TestProgram()
        {
            lock(this)
            {
                needToTest = ProgPool.pop(this);
                if (needToTest == null)
                {
                    __changeTextStatus("Prepearing to write new program because it appears that nothing to test left");
                    __changeStatus(1);
                    return;
                }

                __changeTextStatus($"Start testing {needToTest.writer.name}'s program");

                needToTest.tester = this;
                Thread.Sleep(Util.randInt(3000, 7000));
                needToTest.correct = Util.randBool();
                __changeTextStatus($"Program written by {needToTest.writer.name} is " + (needToTest.correct ? "correct" : "not correct"));
                if (!needToTest.correct)
                {
                    if (needToTest.writer.status == 3)
                    {
                        needToTest.writer.are.Set();
                    }
                }

                __changeStatus(3);
            }
        }

        public void Sleep()
        {
            lock(this)
            {
                __changeTextStatus("Sleeping...");
                if(are.WaitOne(sleepDuration) == true)
                {
                    __changeTextStatus($"{this.name} been awakened");
                    if (ownProgram?.correct == true)
                    {
                        __changeStatus(1);
                    }
                    else
                    {
                        __changeStatus(4);
                    }
                }
                else
                {
                    __changeTextStatus("Just woke up");
                }
                are.Reset();

                __changeStatus(1);
            }
        }


        public void FixOwnProgram()
        {
            lock(this)
            {
                __changeTextStatus("Fixing own program");
                Thread.Sleep(Util.randInt(4000, 6000));
                ownProgram.correct = true;
                ownProgram.beenFixed = true;
            }
        }

        public void TestFixedProgram()
        {
            lock (this)
            {
                __changeTextStatus($"Testing {this.needToTest.writer.name}'s fixed program");
                Thread.Sleep(Util.randInt(3000, 7000));
                bool correct = Util.randBool();
                needToTest.correct = correct;
                needToTest.beenFixed = false;
                __changeTextStatus($"Program fixed by {needToTest.writer.name}'s is " + (correct == true ? "correct" : "not correct"));

                LookAround();
                __changeStatus(3);
            }
        }

        public void LookAround()
        {
            if (ownProgram?.correct == false)
            {
                this.status = 4;
                this.FixOwnProgram();
            }
            if (needToTest?.beenFixed == true)
            {
                this.status = 5;
                this.TestFixedProgram();
            }
        }

        /*
        ################################
                    PRIVATE
        ################################
        */

        private void __changeTextStatus(string text)
        {
            OnTextStatusUpdate(this, text, statusPrefix[this.status - 1]);
        }


        private void __changeStatus(int status)
        {
            lock(this)
            {

                Thread.Sleep(2500);
                __changeTextStatus("Making coffee");
                Thread.Sleep(1000);


                LookAround();
                this.status = status;
                switch (status)
                {
                    case 1:
                        this.MakeOwnProgram();
                        break;
                    case 2:
                        this.TestProgram();
                        break;
                    case 3:
                        this.Sleep();
                        break;
                    case 4:
                        this.FixOwnProgram();
                        break;
                }
            }
        }
    }
}

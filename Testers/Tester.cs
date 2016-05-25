using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//https://github.com/Self747/Testers/blob/master/Testers/Tester.cs
namespace Testers
{
    public delegate void textStatusUpdate(object tester, string message, string prefix);

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

        public string name { private set; get; } = "Неназваний";

        public Prog ownProgram { get; set; } = null;

        public Prog needToTest { get; set; } = null;

        public int index;

        private ManualResetEvent are;

        private int sleepDuration;

        private static string[] statusPrefix = { "[ПИШЕ]", "[ТЕСТУЄ]", "[СПИТЬ]", "[ВИПРАВЛЯЄ]", "[ТЕСТУЄ]" };

        public static Color[] textStatusColors = { Color.DarkBlue, Color.Brown, Color.Teal, Color.Indigo, Color.Brown };

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
                __changeTextStatus("Пише власну програму");
                Thread.Sleep(Util.randInt(4000, 6000));
                ownProgram = new Prog() { writer = this };
                ProgPool.add(ownProgram);
                __changeTextStatus("Закінчив написання програми");

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
                    __changeTextStatus("Пише свою програму, бо нічого тестити");
                    __changeStatus(1);
                    return;
                }

                __changeTextStatus($"Починає тестувати {needToTest.writer.name} програму");

                needToTest.tester = this;
                Thread.Sleep(Util.randInt(3000, 7000));
                needToTest.correct = Util.randBool();
                __changeTextStatus($"Програма написана {needToTest.writer.name} є " + (needToTest.correct ? "правильна" : "не правильна"));
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
                __changeTextStatus("Спить..zzZZz..");
                if(are.WaitOne(sleepDuration) == true)
                {
                    __changeTextStatus($"{this.name} був розбуджений");
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
                    __changeTextStatus("Прокинувся");
                }
                are.Reset();

                __changeStatus(1);
            }
        }


        public void FixOwnProgram()
        {
            lock(this)
            {
                __changeTextStatus("Виправляє власну програму");
                Thread.Sleep(Util.randInt(4000, 6000));
                ownProgram.correct = true;
                ownProgram.beenFixed = true;
            }
        }

        public void TestFixedProgram()
        {
            lock (this)
            {
                __changeTextStatus($"Тестує {this.needToTest.writer.name} виправлену програму");
                Thread.Sleep(Util.randInt(3000, 7000));
                bool correct = Util.randBool();
                needToTest.correct = correct;
                needToTest.beenFixed = false;
                __changeTextStatus($"Виправлена програма {needToTest.writer.name} є " + (correct == true ? "правильна" : "не правильна"));

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
                __changeTextStatus("П'є кавуську");
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

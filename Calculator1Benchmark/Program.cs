using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using MathParserTK;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Attributes;

namespace Calculator1Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {

            var summary = BenchmarkRunner.Run<TestInlining>();


            Console.ReadLine();
        }
    }


    public class TestInlining
    {
        private readonly string _mathOperation = "sin(cos(tg(sh(ch(th(100))))))";
        private readonly int _testCycles = 1000;

        [Benchmark]
        public void RunWithAttribute()
        {
            WithAttributeCalculator test = new WithAttributeCalculator();
            for (int n = 0; n < _testCycles; n++)
            {
                test.Entry_Input = _mathOperation;
            }
        }

        [Benchmark]
        public void RunInlinedNoAttribute()
        {
            InlinedNoAttribute test = new InlinedNoAttribute();
            for (int n = 0; n < _testCycles; n++)
            {
                test.Entry_Input = _mathOperation;
            }
        }

        [Benchmark]
        public void RunNoAttributeCalculator()
        {
            NoAttributeCalculator test = new NoAttributeCalculator();
            for (int n = 0; n < _testCycles; n++)
            {
                test.Entry_Input = _mathOperation;
            }
        }
    }


    class WithAttributeCalculator : INotifyPropertyChanged
    {
        private string label_Output;
        public string Label_Output
        {
            get => label_Output;
            set
            {
                if (label_Output != value) label_Output = value;
                else return;

                OnPropertyChanged();
            }
        }

        private string entry_Input;
        public string Entry_Input
        {
            get => entry_Input;
            set
            {
                if (entry_Input != value) entry_Input = value;
                else return;

                SetOutput(Regex.Replace(entry_Input, " ", ""));

                OnPropertyChanged();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] // this method is only executed inline // this attribute can be delted its just for performance
        private void SetOutput(string entry)
        {

            MathParser mathParser = new MathParser();
            if (!string.IsNullOrEmpty(entry) && char.IsDigit(entry[entry.Length - 1]))
            {
                Label_Output = mathParser.Parse(entry).ToString();
            }
            else
            {
                Label_Output = "Error";
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    class InlinedNoAttribute : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string label_Output;
        public string Label_Output
        {
            get => label_Output;
            set
            {
                if (label_Output != value) label_Output = value;
                else return;

                OnPropertyChanged();
            }
        }

        private string entry_Input;
        public string Entry_Input
        {
            get => entry_Input;
            set
            {
                if (entry_Input != value) entry_Input = value;
                else return;

                SetOutput(Regex.Replace(entry_Input, " ", ""));

                OnPropertyChanged();

                void SetOutput(string entry)
                {

                    MathParser mathParser = new MathParser();
                    if (!string.IsNullOrEmpty(entry) && char.IsDigit(entry[entry.Length - 1]))
                    {
                        Label_Output = mathParser.Parse(entry).ToString();
                    }
                    else
                    {
                        Label_Output = "Error";
                    }
                }

            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    class NoAttributeCalculator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private string label_Output;
        public string Label_Output
        {
            get => label_Output;
            set
            {
                if (label_Output != value) label_Output = value;
                else return;

                OnPropertyChanged();
            }
        }

        private string entry_Input;
        public string Entry_Input
        {
            get => entry_Input;
            set
            {
                if (entry_Input != value) entry_Input = value;
                else return;

                SetOutput(Regex.Replace(entry_Input, " ", ""));

                OnPropertyChanged();
            }
        }

        private void SetOutput(string entry)
        {

            MathParser mathParser = new MathParser();
            if (!string.IsNullOrEmpty(entry) && char.IsDigit(entry[entry.Length - 1]))
            {
                Label_Output = mathParser.Parse(entry).ToString();
            }
            else
            {
                Label_Output = "Error";
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }




}

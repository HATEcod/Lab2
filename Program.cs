using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace QA_Lab2
{
    public class QA_Lab2
    {
        // _exception_List_Source
        IListSource _exc_List_Source;
        //_telemetry_Reporters
        Tel_Reporters _tel_Reporters;
        
            //Critical_Exceptions_Counter
        public int C_E_C { get; private set; }

           //Counter_Not_Critical_Exceptions
        public int C_N_C_E { get; private set; }
            //Report_Failures
        public int R_F { get; private set; }

        public QA_Lab2() { }

        static void Main(string[] args)
        {

        }

        public QA_Lab2(IListSource src, Tel_Reporters reporters)
        {
             _exc_List_Source = src;
             _tel_Reporters = reporters;
        }

        public bool IsCritical(Exception exception)
        {
            var Critical_Exceptions = _exc_List_Source.GetList();
            return Critical_Exceptions.Contains(exception.GetType());
        }

        public void CountExceptions(Exception exception)
        {
            if (IsCritical(exception))
            {
                    C_E_C += 1;
                if (!_tel_Reporters.Report(exception.ToString()))
                {
                    R_F = R_F + 1;
                }
            }
            else
            {
                C_N_C_E += 1;
            }
        }

        public IListSource ExceptionListSource
        {
            set
            {
                _exc_List_Source = value;
            }
        }

        public Tel_Reporters TelemetryReporter
        {
            set
            {
                _tel_Reporters = value;
            }
        }
    }
}
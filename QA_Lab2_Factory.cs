using System.ComponentModel;
using System;

namespace QA_Lab2
{
    public class QA_Lab2_Factory
    {
        private IListSource _listSource;
        private Tel_Reporters _tel_Reporters;

        public QA_Lab2_Factory() { }

        public QA_Lab2_Factory WithListSource(IListSource src)
        {
            _listSource = src;
            return this;
        }

        public QA_Lab2_Factory WithTelemetryReporter(Tel_Reporters reports)
        {
            _tel_Reporters = reports;
            return this;
        }

        public QA_Lab2 Build()
        {
            return new QA_Lab2(_listSource, _tel_Reporters);
        }
    }
}
using System;
using System.ComponentModel;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace QA_Lab2.UnitTests
{
    public class Lab2Test
    {
        //Critical_Exceptions
        public static List<Type> C_E = new List<Type>()
        {
            typeof(DivideByZeroException),
            typeof(OutOfMemoryException),
            typeof(StackOverflowException),
            typeof(InsufficientMemoryException),
            typeof(InsufficientExecutionStackException)
        };
        //Non_Critical_Exceptions
        public static List<Type> N_C_E = new List<Type>()
        {
            typeof(ArgumentNullException),
            typeof(ArgumentOutOfRangeException),
            typeof(NullReferenceException),
            typeof(AccessViolationException),
            typeof(IndexOutOfRangeException),
            typeof(InvalidOperationException)
        };
    }

    [SetUpFixture]
    public class TestSetup
    {
        public static System.ComponentModel.IListSource ListSource;
        //Normal_Telemetry_Reporters
        public static Tel_Reporters N_T_R;
        //Failing_Telemetry_Reporters
        public static Tel_Reporters F_T_R;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ListSource = Substitute.For<IListSource>();
            ListSource.GetList().Returns(Lab2Test.C_E);
            N_T_R = Substitute.For<Tel_Reporters>();
            N_T_R.Report(Arg.Any<String>()).Returns(true);
            F_T_R = Substitute.For<Tel_Reporters>();
            F_T_R.Report(Arg.Any<String>()).Returns(false);
        }
    }
    [TestFixture]
    public class LW2Tests
    {
        [TestCase(typeof(DivideByZeroException), true)]
        [TestCase(typeof(OutOfMemoryException), true)]
        [TestCase(typeof(StackOverflowException), true)]
        [TestCase(typeof(InsufficientMemoryException), true)]
        [TestCase(typeof(InsufficientExecutionStackException), true)]
        [TestCase(typeof(ArgumentNullException), false)]
        [TestCase(typeof(ArgumentOutOfRangeException), false)]
        [TestCase(typeof(NullReferenceException), false)]
        [TestCase(typeof(AccessViolationException), false)]
        [TestCase(typeof(IndexOutOfRangeException), false)]
        [TestCase(typeof(InvalidOperationException), false)]
        public void IsCritical_CriticalityCheck_Correct(Type exceptionType, bool expectedResult)
        {
            // arrange
            var instance = (Exception)Activator.CreateInstance(exceptionType);
            // 1. Use a LW2Factory
            var lab2 = new QA_Lab2_Factory()
                .WithListSource(TestSetup.ListSource)
                //Normal_Telemetry_Reporters
                .WithTelemetryReporter(TestSetup.N_T_R)
                .Build();

            try
            {
                // act
                throw instance;
            }
            catch (Exception e)
            {
                // assert
                Assert.AreEqual(expectedResult, lab2.IsCritical(e));
                return;
            }
        }

        [Test]
        public void CountExceptions_CounterValues_Correct()
        {
            // arrange
            // 2. Use constructor
            var lab2 = new QA_Lab2(TestSetup.ListSource, TestSetup.N_T_R);

            // act
            foreach (var item in Lab2Test.C_E)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab2.CountExceptions(instance);
            }

            foreach (var item in Lab2Test.N_C_E)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab2.CountExceptions(instance);
            }

            // assert
            Assert.AreEqual(lab2.C_E_C, Lab2Test.C_E.Count); //Critical Exceptions Counter
            Assert.AreEqual(lab2.C_N_C_E, Lab2Test.N_C_E.Count);//Counter Not Critical Exceptions
        }

        [Test]
        public void CountExceptions_InitCounters_Zero()
        {
            // arrange
            // 3. Use property access
            var lab2 = new QA_Lab2();
            lab2.ExceptionListSource = TestSetup.ListSource;
            lab2.TelemetryReporter = TestSetup.N_T_R;

            // act: nothing

            // assert
            Assert.AreEqual(lab2.C_E_C, 0);
            Assert.AreEqual(lab2.C_N_C_E, 0);
        }

        [Test]
        public void TelemetryReport_FailureCounter_Correct()
        {
            // arrange
            var lab02 = new QA_Lab2(TestSetup.ListSource, TestSetup.N_T_R);
            var lab01 = new QA_Lab2(TestSetup.ListSource, TestSetup.F_T_R);

            // act
            foreach (var item in Lab2Test.C_E)
            {
                var instance = (Exception)Activator.CreateInstance(item);
                lab02.CountExceptions(instance);
                lab01.CountExceptions(instance);
            }

            // assert
            Assert.AreEqual(lab02.R_F, 0);
            Assert.AreEqual(lab01.R_F, Lab2Test.C_E.Count);
        }

        [Test]
        public void TelemetryReport_InitCounter_Zero()
        {
            // arrange
            var lab2 = new QA_Lab2(TestSetup.ListSource, TestSetup.F_T_R);//Failing Telemetry Reporters

            // act: nothing

            // assert
            Assert.AreEqual(lab2.R_F, 0);//Report Failures
        }
    }
}
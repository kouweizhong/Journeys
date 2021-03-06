﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mors.Journeys.Domain.Expenses.Capabilities;
using Mors.Journeys.Domain.Test;

namespace Mors.Journeys.Domain.Expenses.Test.Capabilities
{
    [TestClass]
    public sealed class LiftIdTest
    {
        [TestMethod]
        public void EqualsShouldReturnTrueForInstancesWithSameJourneyAndPersonIds()
        {
            var a = new LiftId(new Id(0), new Id(1));
            var b = new LiftId(new Id(0), new Id(1));

            var result = a.Equals(b);

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void EqualsShouldReturnFalseForInstancesWithSameJourneyButDifferentPersonIds()
        {
            var a = new LiftId(new Id(0), new Id(1));
            var b = new LiftId(new Id(0), new Id(2));

            var result = a.Equals(b);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EqualsShouldReturnFalseForInstancesWithSamePersonButDifferentJourneyIds()
        {
            var a = new LiftId(new Id(0), new Id(2));
            var b = new LiftId(new Id(1), new Id(2));

            var result = a.Equals(b);

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void EqualsShouldReturnFalseForInstancesWhereSecondIsNotLiftId()
        {
            var a = new LiftId(new Id(0), new Id(1));
            var b = new Id(0);

            var result = a.Equals(b);

            Assert.AreEqual(false, result);
        }
    }
}

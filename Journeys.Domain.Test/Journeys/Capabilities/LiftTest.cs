﻿using System;
using Journeys.Domain.Infrastructure;
using Journeys.Domain.Journeys.Capabilities;
using Journeys.Domain.People;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Journeys.Domain.Test.Journeys.Capabilities
{
    [TestClass]
    public class LiftTest
    {
        public static readonly Id<Person> PersonId = new Id<Person>(new Guid());
        public static readonly Id<Person> AnotherPersonId = new Id<Person>(Guid.NewGuid());

        [TestMethod]
        public void LiftWithPersonIdShouldBeEqualByPersonToThatId()
        {
            var lift = new Lift(PersonId, new Distance(1m, DistanceUnit.Kilometer));

            Assert.IsTrue(lift.EqualsByPerson(PersonId));
        }

        [TestMethod]
        public void LiftWithPersonIdShouldNotBeEqualByPersonToOtherId()
        {
            var lift = new Lift(PersonId, new Distance(1m, DistanceUnit.Kilometer));

            Assert.IsFalse(lift.EqualsByPerson(AnotherPersonId));
        }
    }
}

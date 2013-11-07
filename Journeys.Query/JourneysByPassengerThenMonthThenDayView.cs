﻿using Journeys.Common;
using Journeys.Events;
using Journeys.Queries.Dtos.JourneysByPassengerThenMonthThenDay;
using Journeys.Query.Infrastructure.Views;
using System;
using System.Linq;
using System.Collections.Generic;
using Journeys.Queries;

namespace Journeys.Query
{
    internal class JourneysByPassengerThenMonthThenDayView
    {
        private readonly Journeys.Query.Infrastructure.Views.Lookup<Key, Value> _facts = new Journeys.Query.Infrastructure.Views.Lookup<Key, Value>();
        private readonly Set<IId, JourneyCreatedEvent> _journeys = new Set<IId, JourneyCreatedEvent>(journey => journey.JourneyId);
        private readonly Journeys.Query.Infrastructure.Views.Lookup<DateTime, HashSet<JourneyCreatedEvent>> _journeysByDate = new Journeys.Query.Infrastructure.Views.Lookup<DateTime, HashSet<JourneyCreatedEvent>>();

        public IEnumerable<Fact> Execute(GetJourneysByPassengerThenMonthThenDayQuery query)
        {
            foreach (var factEntry in _facts.Retrieve())
            {
                yield return new Fact(factEntry.Key, factEntry.Value);
            }
        }

        public void Update(JourneyCreatedEvent @event)
        {
            _journeys.Add(@event);
            var journeysOnDateOfOccurence = _journeysByDate.GetOrAdd(@event.DateOfOccurrence, () => new HashSet<JourneyCreatedEvent>());
            journeysOnDateOfOccurence.Add(@event);
        }

        public void Update(LiftAddedEvent @event)
        {
            var journey = _journeys.Get(@event.JourneyId);
            var dateOfOccurrence = journey.DateOfOccurrence;
            var key = new Key(new Passenger(@event.PersonId), new Month(dateOfOccurrence.Year, dateOfOccurrence.Month), new Day(dateOfOccurrence.Day));
            var value = _facts.GetOrAdd(key, () => CreateValue(dateOfOccurrence));
            _facts.Set(key, UpdateValue(value, @event));
        }

        private Value UpdateValue(Value value, LiftAddedEvent @event)
        {
            return new Value(
                value.JourneyCount,
                value.JourneyDistance,
                value.LiftCount + 1,
                value.JourneyDistance + @event.LiftDistance);
        }

        private Value CreateValue(DateTime dateOfOccurrence)
        {
            var journeys = _journeysByDate.Get(dateOfOccurrence).Value;
            return new Value(
                journeys.Count,
                journeys.Sum(j => j.RouteDistance),
                0,
                0m);
        }
    }
}

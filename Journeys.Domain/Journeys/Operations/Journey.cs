﻿using System;
using System.Linq;
using Journeys.Common;
using Journeys.Domain.Infrastructure;
using Journeys.Domain.Infrastructure.Collections;
using Journeys.Domain.Infrastructure.Exceptions;
using Journeys.Domain.Infrastructure.Markers;
using Journeys.Domain.Journeys.Capabilities;
using Journeys.Events;

namespace Journeys.Domain.Journeys.Operations
{
    [Aggregate]
    public class Journey : IHasId
    {
        private readonly IEventBus _eventBus;
        private readonly IId _id;
        private readonly DateTime _dateOfOccurrence;
        private readonly Distance _routeDistance;
        private readonly ImmutableList<Lift> _lifts = ImmutableList<Lift>.Empty;

        public Journey(IId id, DateTime dateOfOccurrence, Distance routeDistance, IEventBus eventBus)
        {
            _dateOfOccurrence = dateOfOccurrence;
            _eventBus = eventBus;
            _id = id;
            _routeDistance = routeDistance;
            _eventBus.Publish(new JourneyCreatedEvent(id, dateOfOccurrence, routeDistance));
        }

        private Journey(Journey journey, ImmutableList<Lift> lifts)
        {
            _dateOfOccurrence = journey._dateOfOccurrence;
            _eventBus = journey._eventBus;
            _id = journey._id;
            _lifts = lifts;
            _routeDistance = journey._routeDistance;
        }

        public Journey AddLift(IId personId, Distance liftDistance)
        {
            if (ContainsLiftForPerson(personId))
                throw new InvariantViolationException(Messages.JourneyAlreadyContainsLiftForThatPerson);
            if (liftDistance > _routeDistance) 
                throw new InvariantViolationException(Messages.CannotAddLiftWithDistanceLargerThanJourneyDistance);
            var lift = new Lift(personId);
            var newLifts = _lifts.Add(lift);
            _eventBus.Publish(new LiftAddedEvent(_id, personId, liftDistance));
            return new Journey(this, newLifts);
        }

        private bool ContainsLiftForPerson(IId personId)
        {
            return _lifts.Any(lift => lift.IsForPerson(personId));
        }

        IId IHasId.Id
        {
            get { return _id; }
        }
    }
}

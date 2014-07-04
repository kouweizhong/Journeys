﻿using System;
using Implementation = Mors.Support.Events;

namespace Journeys.Hosting.Adapters
{
    public class WpfClientEventBus : Application.Client.Wpf.IEventBus
    {
        private readonly Implementation.IEventBus _eventBus;

        public WpfClientEventBus(Implementation.IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void Publish<TEvent>(TEvent @event)
        {
            _eventBus.Publish(@event);
        }

        public void Subscribe<TEvent>(Action<TEvent> listener)
        {
            _eventBus.RegisterListener(new Implementation.EventListener<TEvent>(listener));
        }
    }
}

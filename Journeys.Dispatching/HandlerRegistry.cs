﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journeys.Dispatching
{
    public class HandlerRegistry
    {
        private readonly Dictionary<object, Func<object, object>> _handlers;

        public HandlerRegistry()
        {
            _handlers = new Dictionary<object, Func<object, object>>();
        }

        public void Set(object key, Func<object, object> handler)
        {
            _handlers[key] = handler;
        }

        public bool Retrieve(object key, out Func<object, object> handler)
        {
            return _handlers.TryGetValue(key, out handler);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Events
{
    public record OrderUpdatedEvent(Order order) : IDomainEvent;
}

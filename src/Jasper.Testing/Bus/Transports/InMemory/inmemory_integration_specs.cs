﻿using System;
using System.Threading.Tasks;
using Baseline.Dates;
using Jasper.Bus;
using Jasper.Bus.Transports.Loopback;
using Jasper.Testing.Bus.Runtime;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace Jasper.Testing.Bus.Transports.InMemory
{
    public class inmemory_integration_specs : IntegrationContext
    {
        private readonly MessageTracker theTracker = new MessageTracker();

        public inmemory_integration_specs()
        {
            with(_ =>
            {
                _.Publish.Message<Message1>().To("loopback://incoming");

                _.Services.AddSingleton(theTracker);

                _.Services.Scan(x =>
                {
                    x.TheCallingAssembly();
                    x.WithDefaultConventions();
                });
            });
        }

        [Fact]
        public void automatically_sticks_in_replies_queue()
        {
            Channels.HasChannel(LoopbackTransport.Retries)
                .ShouldBeTrue();
        }


        [Fact]
        public async Task send_a_message_and_get_the_response()
        {
            var bus = Runtime.Get<IServiceBus>();

            var task = theTracker.WaitFor<Message1>();

            await bus.Send(new Message1());

            task.Wait(20.Seconds());

            if (!task.IsCompleted)
            {
                throw new Exception("Got no envelope!");
            }

            var envelope = task.Result;

            envelope.ShouldNotBeNull();
        }

    }
}

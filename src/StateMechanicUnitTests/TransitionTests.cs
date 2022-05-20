﻿using NUnit.Framework;
using StateMechanic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMechanicUnitTests
{
    [TestFixture]
    public class TransitionTests
    {
        [Test]
        public void FirstRegisteredTransitionWins()
        {
            var sm = new StateMachine("state machine");
            var initial = sm.CreateInitialState("initial");
            var state1 = sm.CreateState("state1");
            var state2 = sm.CreateState("state2");

            var evt = new Event("evt");

            initial.TransitionOn(evt).To(state2).WithGuard(i => true);
            initial.TransitionOn(evt).To(state1);

            evt.Fire();

            Assert.AreEqual(state2, sm.CurrentState);
        }

        [Test]
        public void FirstRegisteredTransitionWithTrueGuardWins()
        {
            var sm = new StateMachine("state machine");
            var initial = sm.CreateInitialState("initial");
            var state1 = sm.CreateState("state1");
            var state2 = sm.CreateState("state2");
            var state3 = sm.CreateState("state3");
            var state4 = sm.CreateState("state4");

            var evt = new Event("evt");

            initial.TransitionOn(evt).To(state1).WithGuard(i => false);
            initial.TransitionOn(evt).To(state2).WithGuard(i => false);
            initial.TransitionOn(evt).To(state3).WithGuard(i => true);
            initial.TransitionOn(evt).To(state4);

            evt.Fire();

            Assert.AreEqual(state3, sm.CurrentState);
        }

        [Test]
        public void TransitionIsAbortedIfAnyGuardThrowsAnException()
        {
            var sm = new StateMachine("sm");
            var initial = sm.CreateInitialState("initial");
            var state1 = sm.CreateState("state1");
            var evt = new Event("evt");

            var exception = new Exception("foo");
            initial.TransitionOn(evt).To(initial).WithGuard(i => { throw exception; });
            initial.TransitionOn(evt).To(state1);

            var e = Assert.Throws<Exception>(() => evt.Fire());
            Assert.AreEqual(exception, e);
            Assert.AreEqual(initial, sm.CurrentState);
        }

        [Test]
        public void NoEventsAreQueuedIfAGuardThrowsAnException()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");
            var state2 = sm.CreateState("state2");

            var evt = new Event("evt");
            var evt2 = new Event("evt2");

            var exception = new Exception("foo");
            state1.TransitionOn(evt).To(state2).WithGuard(i =>
            {
                evt.Fire();
                throw exception;
            });
            state1.TransitionOn(evt2).To(state2);
            state2.TransitionOn(evt).To(state1);

            Assert.Throws<Exception>(() => evt.Fire());

            // Make sure that an event wasn't queued
            evt2.Fire();

            Assert.AreEqual(state2, sm.CurrentState);
        }
    }
}

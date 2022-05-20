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
    public class AssertionTests
    {
        [Test]
        public void ThrowsIfEventUsedOnTwoDifferentStateMachines()
        {
            var sm1 = new StateMachine("State Machine 1");
            var state1 = sm1.CreateInitialState("Initial State");

            var sm2 = new StateMachine("State Machine 2");
            var state2 = sm2.CreateInitialState("Initial State");

            var evt = new Event("Event");

            state1.TransitionOn(evt).To(state1);

            var e = Assert.Throws<InvalidEventTransitionException>(() => state2.TransitionOn(evt).To(state2));
            Assert.AreEqual(state2, e.From);
            Assert.AreEqual(evt, e.Event);
        }

        [Test]
        public void ThrowsIfEventFiredAndInitialStateNotSet()
        {
            var sm = new StateMachine("State Machine");
            var state1 = sm.CreateState("Initial State");
            var evt = new Event("Event");

            state1.InnerSelfTransitionOn(evt);

            Assert.Throws<InvalidOperationException>(() => evt.Fire());
        }

        [Test]
        public void ThrowsIfInitialStateSetTwice()
        {
            var sm = new StateMachine("State machine");
            var state1 = sm.CreateInitialState("State 1");
            Assert.Throws<InvalidOperationException>(() => sm.CreateInitialState("State 2"));
        }

        [Test]
        public void ThrowsIfTransitionOnIsNull()
        {
            var sm = new StateMachine("State machine");
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => state1.TransitionOn(null));
        }

        [Test]
        public void ThrowsIfTransitionOnWithEventDataIsNull()
        {
            var sm = new StateMachine("State machine");
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => state1.TransitionOn<string>(null));
        }

        [Test]
        public void ThrowsIfInnerSelfTransitionOnIsNull()
        {
            var sm = new StateMachine("State machine");
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => state1.InnerSelfTransitionOn(null));
        }

        [Test]
        public void ThrowsIfInnerSelfTransitionOnWithEventDataIsNull()
        {
            var sm = new StateMachine("State machine");
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => state1.InnerSelfTransitionOn<string>(null));
        }

        [Test]
        public void StateThrowsIfAddToGroupCalledWithNull()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => state1.AddToGroup(null));
        }

        [Test]
        public void StateThrowsIfAddToGroupsCalledWithNull()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => state1.AddToGroups(null));
        }

        [Test]
        public void StateGroupThrowsIfAddStateCalledWithNull()
        {
            var group = new StateGroup();
            Assert.Throws<ArgumentNullException>(() => group.AddState(null));
        }

        [Test]
        public void StateGroupThrowsIfAddStatesCalledWithNull()
        {
            var group = new StateGroup();
            Assert.Throws<ArgumentNullException>(() => group.AddStates(null));
        }

        [Test]
        public void ForceTransitionThrowsIfToStateIsNull()
        {
            var sm = new StateMachine();
            var evt = new Event("evt");
            Assert.Throws<ArgumentNullException>(() => sm.ForceTransition(null, evt));
        }

        [Test]
        public void ForceTransitionThrowsIfEventIsNull()
        {
            var sm = new StateMachine();
            var state1 = sm.CreateInitialState("state1");
            Assert.Throws<ArgumentNullException>(() => sm.ForceTransition(state1, null));
        }

        [Test]
        public void TransitionToThrowsIfStateIsNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");
            var evt = new Event("evt");

            var builder = initial.TransitionOn(evt);
            Assert.Throws<ArgumentNullException>(() => builder.To(null));
        }

        [Test]
        public void TransitionToThrowsIfDynamicIsNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");
            var evt = new Event("evt");

            var builder = initial.TransitionOn(evt);
            Assert.Throws<ArgumentNullException>(() => builder.ToDynamic(null));
        }

        [Test]
        public void TransitionToWithEventDataThrowsIfStateIsNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");
            var evt = new Event<string>("evt");

            var builder = initial.TransitionOn(evt);
            Assert.Throws<ArgumentNullException>(() => builder.To(null));
        }

        [Test]
        public void StateIgnoreThrowsIfEventIsNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");

            Assert.Throws<ArgumentNullException>((() => initial.Ignore((Event)null)));
        }

        [Test]
        public void StateIgnoreThrowsIfEventTIsNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");

            Assert.Throws<ArgumentNullException>((() => initial.Ignore((Event<string>)null)));
        }

        [Test]
        public void StateIgnoreThrowsIfEventsAreNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");

            Assert.Throws<ArgumentNullException>(() => initial.Ignore((Event[])null));
        }

        [Test]
        public void StateIgnoreThrowsIfEventsTsAreNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");

            Assert.Throws<ArgumentNullException>(() => initial.Ignore((Event<string>[])null));
        }

        [Test]
        public void TransitionToWithEventDataThrowsIfDynamicIsNull()
        {
            var sm = new StateMachine();
            var initial = sm.CreateInitialState("initial");
            var evt = new Event<string>("evt");

            var builder = initial.TransitionOn(evt);
            Assert.Throws<ArgumentNullException>(() => builder.ToDynamic(null));
        }

        [Test]
        public void ThrowsIfTransitionAddedAfterAnUnconditionalTransition()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");
            var state2 = sm.CreateState("state2");

            var evt = new Event("evt");

            state1.TransitionOn(evt).To(state2);
            Assert.Throws<ArgumentException>(() => state1.TransitionOn(evt).To(state1));
        }

        [Test]
        public void DoesNotThrowIfTransitionAddedAfterAGuardedTransition()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");
            var state2 = sm.CreateState("state2");

            var evt = new Event("evt");

            state1.TransitionOn(evt).To(state2).WithGuard(i => true);
            Assert.DoesNotThrow(() => state1.TransitionOn(evt).To(state1));
        }

        [Test]
        public void DoesNotThrowIfTransitionAddedAfterADynamicTransition()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");
            var state2 = sm.CreateState("state2");

            var evt = new Event("evt");

            state1.TransitionOn(evt).ToDynamic(i => null);
            Assert.DoesNotThrow(() => state1.TransitionOn(evt).To(state1));
        }

        [Test]
        public void ThrowsIfATransitionAddedAfterAnIgnoredEvent()
        {
            var sm = new StateMachine("sm");
            var state1 = sm.CreateInitialState("state1");

            var evt = new Event("evt");

            state1.Ignore(evt);
            Assert.Throws<ArgumentException>(() => state1.TransitionOn(evt).To(state1));
        }
    }
}

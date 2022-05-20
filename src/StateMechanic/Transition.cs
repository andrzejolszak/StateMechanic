﻿using System;

namespace StateMechanic
{

    /// <summary>
    /// A transition from one state to another, triggered by an eventf
    /// </summary>
    /// <typeparam name="TState">Type of state which this transition is between</typeparam>

    public class Transition<TState> : ITransition<TState>, IInvokableTransition

        where TState : StateBase<TState>, new()
    {
        private readonly ITransitionDelegate<TState> transitionDelegate;

        /// <summary>
        /// Gets the state this transition is from
        /// </summary>
        public TState From { get; }

        /// <summary>
        /// Gets the state this transition to
        /// </summary>
        public TState To { get; }

        /// <summary>
        /// Gets the event which triggers this transition
        /// </summary>
        public Event Event { get; }
        IEvent ITransition<TState>.Event => this.Event;

        bool ITransition<TState>.IsDynamicTransition => false;
        bool ITransition.WillAlwaysOccur => !this.HasGuard;

        /// <summary>
        /// Gets a value indicating whether this transition is an inner transition, i.e. whether the <see cref="From"/> and <see cref="To"/> states are the same, and no exit/entry handles are invoked
        /// </summary>
        public bool IsInnerTransition { get; }

        /// <summary>
        /// Gets or sets a method which is invoked whenever this transition occurs
        /// </summary>
        public Action<TransitionInfo<TState>> Handler { get; set; }

         /// <summary>
        /// Gets or sets a method which is invoked before this transition occurs, and can prevent the transition from occuring
        /// </summary>
        public Func<TransitionInfo<TState>, bool> Guard { get; set; }

        /// <summary>
        /// Gets a value indicating whether this transition has a guard
        /// </summary>
        public bool HasGuard => this.Guard != null;

        internal Transition(TState from, TState to, Event @event, ITransitionDelegate<TState> transitionDelegate)
            : this(from, to, @event, transitionDelegate, isInnerTransition: false)
        {
        }
        
        internal Transition(TState fromAndTo, Event @event, ITransitionDelegate<TState> transitionDelegate)
            : this(fromAndTo, fromAndTo, @event, transitionDelegate, isInnerTransition: true)
        {
        }

        private Transition(TState from, TState to, Event @event, ITransitionDelegate<TState> transitionDelegate, bool isInnerTransition)
        {
            if (from.ParentStateMachine != to.ParentStateMachine)
                throw new InvalidStateTransitionException(from, to);

            this.transitionDelegate = transitionDelegate;
            this.From = from;
            this.To = to;
            this.Event = @event;
            this.IsInnerTransition = isInnerTransition;
        }

        /// <summary>
        /// Sets a method which is invoked whenever this transition occurs
        /// </summary>
        /// <param name="handler">Method which is invoked whenever this transition occurs</param>
        /// <returns>This transition, for method chaining</returns>
        public Transition<TState> WithHandler(Action<TransitionInfo<TState>> handler)
        {
            this.Handler = handler;
            return this;
        }

        /// <summary>
        /// Sets a method which is invoked before this transition occurs, and can prevent the transition from occuring
        /// </summary>
        /// <param name="guard">method which is invoked before this transition occurs, and can prevent the transition from occuring</param>
        /// <returns>This transition, for method chaining</returns>
        public Transition<TState> WithGuard(Func<TransitionInfo<TState>, bool> guard)
        {
            this.Guard = guard;
            return this;
        }


        bool IInvokableTransition.TryInvoke(EventFireMethod eventFireMethod)

        {
            if (!this.From.CanTransition(this.Event, this.To, null))
                return false;

            var transitionInfo = new TransitionInfo<TState>(this.From, this.To, this.Event, this.IsInnerTransition, eventFireMethod);

            var guard = this.Guard;
            if (guard != null && !guard(transitionInfo))
                return false;

            this.transitionDelegate.CoordinateTransition(transitionInfo, this.Handler);

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        [ExcludeFromCoverage]
        public override string ToString()
        {
            return String.Format("<Transition From={0} To={1} Event={2}{3}>", this.From.Name ?? "(unnamed)", this.To.Name ?? "(unnamed)", this.Event.Name ?? "(unnamed)", this.IsInnerTransition ? " IsInnerTransition" : "");
        }
    }



    /// <summary>
    /// A transition from one state to another, triggered by an eventf
    /// </summary>
    /// <typeparam name="TState">Type of state which this transition is between</typeparam>

    /// <typeparam name="TEventData">Type of event data associated with the event which triggers this transition</typeparam>
    public class Transition<TState, TEventData> : ITransition<TState>, IInvokableTransition<TEventData>

        where TState : StateBase<TState>, new()
    {
        private readonly ITransitionDelegate<TState> transitionDelegate;

        /// <summary>
        /// Gets the state this transition is from
        /// </summary>
        public TState From { get; }

        /// <summary>
        /// Gets the state this transition to
        /// </summary>
        public TState To { get; }

        /// <summary>
        /// Gets the event which triggers this transition
        /// </summary>
        public Event<TEventData> Event { get; }
        IEvent ITransition<TState>.Event => this.Event;

        bool ITransition<TState>.IsDynamicTransition => false;
        bool ITransition.WillAlwaysOccur => !this.HasGuard;

        /// <summary>
        /// Gets a value indicating whether this transition is an inner transition, i.e. whether the <see cref="From"/> and <see cref="To"/> states are the same, and no exit/entry handles are invoked
        /// </summary>
        public bool IsInnerTransition { get; }

        /// <summary>
        /// Gets or sets a method which is invoked whenever this transition occurs
        /// </summary>
        public Action<TransitionInfo<TState, TEventData>> Handler { get; set; }

         /// <summary>
        /// Gets or sets a method which is invoked before this transition occurs, and can prevent the transition from occuring
        /// </summary>
        public Func<TransitionInfo<TState, TEventData>, bool> Guard { get; set; }

        /// <summary>
        /// Gets a value indicating whether this transition has a guard
        /// </summary>
        public bool HasGuard => this.Guard != null;

        internal Transition(TState from, TState to, Event<TEventData> @event, ITransitionDelegate<TState> transitionDelegate)
            : this(from, to, @event, transitionDelegate, isInnerTransition: false)
        {
        }
        
        internal Transition(TState fromAndTo, Event<TEventData> @event, ITransitionDelegate<TState> transitionDelegate)
            : this(fromAndTo, fromAndTo, @event, transitionDelegate, isInnerTransition: true)
        {
        }

        private Transition(TState from, TState to, Event<TEventData> @event, ITransitionDelegate<TState> transitionDelegate, bool isInnerTransition)
        {
            if (from.ParentStateMachine != to.ParentStateMachine)
                throw new InvalidStateTransitionException(from, to);

            this.transitionDelegate = transitionDelegate;
            this.From = from;
            this.To = to;
            this.Event = @event;
            this.IsInnerTransition = isInnerTransition;
        }

        /// <summary>
        /// Sets a method which is invoked whenever this transition occurs
        /// </summary>
        /// <param name="handler">Method which is invoked whenever this transition occurs</param>
        /// <returns>This transition, for method chaining</returns>
        public Transition<TState, TEventData> WithHandler(Action<TransitionInfo<TState, TEventData>> handler)
        {
            this.Handler = handler;
            return this;
        }

        /// <summary>
        /// Sets a method which is invoked before this transition occurs, and can prevent the transition from occuring
        /// </summary>
        /// <param name="guard">method which is invoked before this transition occurs, and can prevent the transition from occuring</param>
        /// <returns>This transition, for method chaining</returns>
        public Transition<TState, TEventData> WithGuard(Func<TransitionInfo<TState, TEventData>, bool> guard)
        {
            this.Guard = guard;
            return this;
        }


        bool IInvokableTransition<TEventData>.TryInvoke(TEventData eventData, EventFireMethod eventFireMethod)

        {
            if (!this.From.CanTransition(this.Event, this.To, eventData))
                return false;

            var transitionInfo = new TransitionInfo<TState, TEventData>(this.From, this.To, this.Event, eventData, this.IsInnerTransition, eventFireMethod);

            var guard = this.Guard;
            if (guard != null && !guard(transitionInfo))
                return false;

            this.transitionDelegate.CoordinateTransition(transitionInfo, this.Handler);

            return true;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        [ExcludeFromCoverage]
        public override string ToString()
        {
            return String.Format("<Transition From={0} To={1} Event={2}{3}>", this.From.Name ?? "(unnamed)", this.To.Name ?? "(unnamed)", this.Event.Name ?? "(unnamed)", this.IsInnerTransition ? " IsInnerTransition" : "");
        }
    }

}


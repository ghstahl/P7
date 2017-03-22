﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using P7.Core.Reflection;
using System.Text;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Validation;

namespace P7.GraphQLCore.Validators
{

    public class EnterLeaveListenerState
    {

        public EnterLeaveListenerState(OperationType operationType, string currentFieldPath)
        {
            OperationType = operationType;
            CurrentFieldPath = currentFieldPath;
        }

        public OperationType OperationType { get; private set; }
        public string CurrentFieldPath { get; private set; }
    }

    public interface IEventSource<T>
    {
        void RegisterEventSink(T sink);
        void UnregisterEventSink(T sink);
    }
    public interface IEnterLeaveListenerEventSink
    {
        void OnEvent(EnterLeaveListenerState enterLeaveListenerState);
    }
    public class MyEnterLeaveListener : INodeVisitor, IEventSource<IEnterLeaveListenerEventSink>
    {
        private OperationType OperationType { get; set; }
        private List<IEnterLeaveListenerEventSink> _eventSinks;

        private List<IEnterLeaveListenerEventSink> EventSinks
        {
            get { return _eventSinks ?? (_eventSinks = new List<IEnterLeaveListenerEventSink>()); }
            set { _eventSinks = value; }
        } 

        private Stack<string> _runningPath;

        private Stack<string> RunningPath => _runningPath ?? (_runningPath = new Stack<string>());

        public string CurrentFieldPath
        {
            get { return (RunningPath.Any() ? RunningPath.Peek() : ""); }
        }

        private readonly List<MatchingNodeListener> _listeners =
            new List<MatchingNodeListener>();

        public MyEnterLeaveListener(Action<MyEnterLeaveListener> configure)
        {
            configure(this);
        }

        public void Enter(INode node)
        {
            var isField = TypeHelper<Field>.IsType(node.GetType());
            var isOperation = TypeHelper<Operation>.IsType(node.GetType());

            if (isOperation)
            {
                var operation = node as Operation;
                OperationType = operation.OperationType;
                RunningPath.Clear();
                FireEnterLeaveListenerState(new EnterLeaveListenerState(OperationType, CurrentFieldPath));
            }
            if (isField)
            {
                var field = node as Field;
                var next = CurrentFieldPath + "/" + field.Name;
                RunningPath.Push(next);
                FireEnterLeaveListenerState(new EnterLeaveListenerState(OperationType, CurrentFieldPath));
            }
            _listeners
                .Where(l => l.Enter != null && l.Matches(node))
                .Apply(l => l.Enter(node));
        }

        public void Leave(INode node)
        {
            var isField = TypeHelper<Field>.IsType(node.GetType());
            if (isField)
            {
                var field = node as Field;
                RunningPath.Pop();
                FireEnterLeaveListenerState(new EnterLeaveListenerState(OperationType, CurrentFieldPath));
            }
            _listeners
                .Where(l => l.Leave != null && l.Matches(node))
                .Apply(l => l.Leave(node));
        }
        public void Match<T>(
           Action<T> enter = null,
           Action<T> leave = null)
           where T : INode
        {
            if (enter == null && leave == null)
            {
                throw new ExecutionError("Must provide an enter or leave function.");
            }

            Func<INode, bool> matches = n => n.GetType().IsAssignableFrom(typeof(T));

            var listener = new MatchingNodeListener
            {
                Matches = matches
            };

            if (enter != null)
            {
                listener.Enter = n => enter((T)n);
            }

            if (leave != null)
            {
                listener.Leave = n => leave((T)n);
            }

            _listeners.Add(listener);
        }

        private void FireEnterLeaveListenerState(EnterLeaveListenerState state)
        {
            foreach (var eventSink in EventSinks)
            {
                eventSink.OnEvent(state);
            }
        }
        public void RegisterEventSink(IEnterLeaveListenerEventSink sink)
        {
            var query = from item in EventSinks
                where item == sink
                select item;

            if (!query.Any())
            {
                EventSinks.Add(sink);
            }
        }

        public void UnregisterEventSink(IEnterLeaveListenerEventSink sink)
        {
            
            var query = from item in EventSinks
                        where item != sink
                        select item;
            EventSinks = query.ToList();
            
        }
    }
}

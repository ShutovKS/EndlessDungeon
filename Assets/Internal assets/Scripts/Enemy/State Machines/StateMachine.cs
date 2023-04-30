using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.State_Machines
{
    public class StateMachine
    {
        /// <summary>
        /// Текущее состояние
        /// </summary>
        private IState _currentState;

        /// <summary>
        /// Словарь всех переходов
        /// </summary>
        private Dictionary<Type, List<Transition>> _transitions = new();

        /// <summary>
        /// Список переходов из текущего состояния
        /// </summary>
        private List<Transition> _currentTransitions = new();

        /// <summary>
        /// Список переховов из любого состояния
        /// </summary>
        private List<Transition> _anyTransitions = new();

        private static readonly List<Transition> EmptyTransitions = new(0);

        /// <summary>
        /// Обновление каждый кадр (осуществление перехода если возможно)
        /// </summary>
        public void Tick()
        {
            var transition = GetTransition();
            if (transition != null) SetState(transition.To);

            _currentState?.Tick();
        }

        /// <summary>
        /// Установить состояние
        /// </summary>
        /// <param name="state">Конечное состояние</param>
        public void SetState(IState state)
        {
            if (state == _currentState)
                return;

            _currentState?.OnExit();
            _currentState = state;
            Debug.Log(_currentState.GetType().Name);
            _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions); // переход
            if (_currentTransitions == null)
                _currentTransitions = EmptyTransitions;

            _currentState.OnEnter();
        }

        /// <summary>
        /// Добавить переход
        /// </summary>
        /// <param name="from">Начальное состояние</param>
        /// <param name="to">Конечное состояние</param>
        /// <param name="predicate">Условие перехода</param>
        public void AddTransition(IState to, IState from, Func<bool> predicate)
        {
            if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
            {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        /// <summary>
        /// Добавить переход с любого состояния
        /// </summary>
        /// <param name="state">Конечное состояние</param>
        /// <param name="predicate">Условие перехода</param>
        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            _anyTransitions.Add(new Transition(state, predicate));
        }

        /// <summary>
        /// Переход к состоянию
        /// </summary>
        private class Transition
        {
            public Func<bool> Condition { get; }
            public IState To { get; }

            public Transition(IState to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        /// <summary>
        /// Перебираются переходы, проверяются на удовлетворённость условия
        /// </summary>
        /// <returns>Переход с удовлетворённым условием</returns>
        private Transition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Condition())
                    return transition;

            foreach (var transition in _currentTransitions)
                if (transition.Condition())
                    return transition;

            return null;
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using State = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP {
    public abstract class Action {
        public float cost = 1F;
        public GameObject target;

        protected bool isDone;

        HashSet<State> mPreconditions = new HashSet<State>();
        HashSet<State> mEffects = new HashSet<State>();
        bool isInRange = false;

        public void Reset() {
            isInRange = false;
            target = null;
            ResetImpl();
        }

        public bool IsDone() {
            return isDone;
        }

        public bool IsInRange() {
            return isInRange;
        }

        // Preconditions and effects
        public void AddPrecondition(string key, object value) {
            mPreconditions.Add(new State(key, value));
        }

        public void RemovePrecondition(string key) {
            State removed = default(State);
            foreach(State p in mPreconditions) {
                if (p.Key.Equals(key))
                    removed = p;
            }
            if (!default(State).Equals(removed))
                mPreconditions.Remove(removed);
        }

        public void AddEffect(string key, object value) {
            mEffects.Add(new State(key, value));
        }

        public void RemoveEffect(string key) {
            State removed = default(State);
            foreach (State e in mEffects) {
                if (e.Key.Equals(key))
                    removed = e;
            }
            if (!default(State).Equals(removed))
                mEffects.Remove(removed);
        }

        public HashSet<State> GetPreconditions() {
            return mPreconditions;
        }

        public HashSet<State> GetEffects() {
            return mEffects;
        }

        // Virtual Methods
        public virtual bool CanRun() { return true; }
        protected virtual void ResetImpl() { }

        // Abstract Methods
        public abstract bool IsRanged();
        public abstract bool Perform(GameObject pAgent);
    }
}

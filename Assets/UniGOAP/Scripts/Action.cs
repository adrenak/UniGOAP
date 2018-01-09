using UnityEngine;
using System.Collections.Generic;
using State = System.Collections.Generic.KeyValuePair<string, object>;

namespace UniLife.GOAP {
    public abstract class Action : MonoBehaviour {
        public float cost = 1F;
        public GameObject target;
        public float range = 0.5f; // Setting this to zero will lead to the agent never reaching it

        protected bool isDone;

        HashSet<State> mPreconditions = new HashSet<State>();
        HashSet<State> mEffects = new HashSet<State>();
        bool mIsInRange = false;

        // ================================================
        // PUBLIC METHODS
        // ================================================
        public Action() {
            Init();
            SetupStateFlags();
        }

        public void Reset() {
            mIsInRange = false;
            target = null;
            isDone = false;
            Init();
        }

        public bool IsDone() {
            return isDone;
        }

        public bool IsInRange() {
            return mIsInRange;
        }

        public void SetInRange(bool pStatus) {
            mIsInRange = pStatus;
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
        public virtual bool CheckProceduralPrecondition(Agent pAgent) { return true; }

        // Abstract Methods
        protected abstract void Init();
        public abstract bool IsRanged();
        public abstract bool Perform(GameObject pAgent);
        public abstract void SetupStateFlags();

        // MISC
        public override string ToString() {
            string s = GetType().Name;
            return s;
        }
    }
}

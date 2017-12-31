using UnityEngine;
using System.Collections.Generic;

namespace UniLife.GOAP {
    public class FSM {
        public delegate void State(FSM fsm, GameObject gameObject);

        Stack<State> mStateStack = new Stack<State>();

        public void Update(GameObject gameObject) {
            if (mStateStack.Peek() != null)
                mStateStack.Peek().Invoke(this, gameObject);
        }

        public void ReplaceState(State state) {
            mStateStack.Pop();
            mStateStack.Push(state);
        }

        public void PushState(State state) {
            mStateStack.Push(state);
        }

        public void PopState() {
            mStateStack.Pop();
        }
    }
}

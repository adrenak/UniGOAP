using System.Collections.Generic;

namespace UniFSM.DelegateBased {
    public class FSM {
        private Stack<State> stateStack = new Stack<State>();
        public delegate void State(FSM fsm, object owner);
        object owner;

        public FSM(object owner) {
            this.owner = owner;
            FSMUpdater.Create();
            FSMUpdater.onUpdate += HandleOnUpdate;
        }

        void HandleOnUpdate() {
            if (stateStack.Peek() != null)
                stateStack.Peek().Invoke(this, owner);
        }

        public void PushState(State state) {
            stateStack.Push(state);
        }

        public void PopState() {
            if(stateStack.Count > 0)
                stateStack.Pop();
        }

        public void PopAndPush(State state) {
            PopState();
            PushState(state);
        }
    }
}

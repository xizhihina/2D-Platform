using System.Collections;
using Myd.Common;
using UnityEngine;

/*
 * 有限状态机
 */
namespace Myd.Platform
{
    public enum EActionState
    {
        Normal,
        Dash,
        Climb,
        Size,
    }
    /// <summary>
    /// 状态基类
    /// 定义了一个抽象类BaseActionState，它包含了状态机中每个状态的基本信息和行为。
    /// 每个状态都有一个关联的EActionState枚举值和一个PlayerController上下文对象。
    /// Update(float deltaTime): 每一帧执行的逻辑，返回当前状态的下一个状态。
    /// Coroutine(): 返回一个IEnumerator，用于执行协程逻辑。
    /// OnBegin(): 当状态开始时调用的方法。
    /// OnEnd(): 当状态结束时调用的方法。
    /// IsCoroutine(): 检查当前状态是否包含协程逻辑。
    /// </summary>
    public abstract class BaseActionState
    {
        protected EActionState state;
        protected PlayerController ctx;

        protected BaseActionState(EActionState state, PlayerController context)
        {
            this.state = state;
            ctx = context;
        }

        public EActionState State { get => state; }

        //每一帧都执行的逻辑
        public abstract EActionState Update(float deltaTime);
        
        //TODO:协程，当前未实现
        public abstract IEnumerator Coroutine();

        public abstract void OnBegin();

        public abstract void OnEnd();

        public abstract bool IsCoroutine();
    }

    /// <summary>
    /// 有限状态机
    /// </summary>
    public class FiniteStateMachine<S> where S : BaseActionState
    {
        private S[] states;

        private int currState = -1;
        private int prevState = -1;
        private Coroutine currentCoroutine;

        public FiniteStateMachine(int size)
        {
            states = new S[size];
            currentCoroutine = new Coroutine();
        }

        public void AddState(S state)
        {
            states[(int)state.State] = state;
        }

        public void Update(float deltaTime)
        {
            State = (int)states[currState].Update(deltaTime);
            if (currentCoroutine.Active)
            {
                currentCoroutine.Update(deltaTime);
            }
        }
        /// <summary>
        /// State属性用于获取和设置状态机的当前状态。在设置新状态时，会触发状态转换的逻辑，包括记录状态转换、调用结束和开始方法，以及管理协程。
        /// </summary>
        public int State
        {
            get
            {
                return currState;
            }
            set
            {
                if (currState == value)
                    return;
                prevState = currState;
                currState = value;
                Debug.Log($"====Enter State[{(EActionState)currState}],Leave State[{(EActionState)prevState}] ");
                if (prevState != -1)
                {
                    Debug.Log($"====State[{(EActionState)prevState}] OnEnd ");
                    states[prevState].OnEnd();
                }
                Debug.Log($"====State[{(EActionState)currState}] OnBegin ");
                states[currState].OnBegin();
                if (states[currState].IsCoroutine())
                {
                    currentCoroutine.Replace(states[currState].Coroutine());
                    return;
                }
                currentCoroutine.Cancel();
            }
        }
    }
}

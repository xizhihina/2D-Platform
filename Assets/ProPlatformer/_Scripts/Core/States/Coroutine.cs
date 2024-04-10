using System.Collections;
using System.Collections.Generic;

namespace Myd.Platform
{
    public class Coroutine
    {
		public bool Finished { get; private set; }
		public bool Active { get; set; }
		public Coroutine(IEnumerator functionCall, bool removeOnComplete = true)
		{
			enumerators = new Stack<IEnumerator>();
			enumerators.Push(functionCall);
			Active = true;
			RemoveOnComplete = removeOnComplete;
		}

		public Coroutine(bool removeOnComplete = true)
		{
			RemoveOnComplete = removeOnComplete;
			enumerators = new Stack<IEnumerator>();
			Active = false;
		}

		public void Update(float deltaTime)
		{
			ended = false;
			if (waitTimer > 0f)
			{
				waitTimer -= deltaTime;
				return;
			}
			if (enumerators.Count > 0)
			{
				IEnumerator enumerator = enumerators.Peek();
				if (enumerator.MoveNext() && !ended)
				{
					if (enumerator.Current is int)
					{
						waitTimer = (int)enumerator.Current;
					}
					if (enumerator.Current is float)
					{
						waitTimer = (float)enumerator.Current;
						return;
					}
					if (enumerator.Current is IEnumerator)
					{
						enumerators.Push(enumerator.Current as IEnumerator);
					}
				}
				else if (!ended)
				{
					enumerators.Pop();
					if (enumerators.Count == 0)
					{
						Active = false;
						Finished = true;
					}
				}
			}
		}

		public void Cancel()
		{
			Active = false;
			Finished = true;
			waitTimer = 0f;
			enumerators.Clear();
			ended = true;
		}

		public void Replace(IEnumerator functionCall)
		{
			Active = true;
			Finished = false;
			waitTimer = 0f;
			enumerators.Clear();
			enumerators.Push(functionCall);
			ended = true;
		}

		public bool RemoveOnComplete = true;

		public bool UseRawDeltaTime;

		private Stack<IEnumerator> enumerators;

		private float waitTimer;

		private bool ended;
	}
}

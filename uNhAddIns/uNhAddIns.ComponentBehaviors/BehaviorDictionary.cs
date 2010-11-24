
using System;
using System.Collections.Generic;

namespace uNhAddIns.ComponentBehaviors
{
	public class BehaviorDictionary : IBehaviorStore
    {
        private readonly IDictionary<Type, BehaviorList> _behaviors =
			new Dictionary<Type, BehaviorList>();

		
        public ICollection<Type> GetBehaviorsForType(Type type)
        {
			BehaviorList result;
            if(_behaviors.TryGetValue(type, out result))
				return result.GetBehaviors();
        	return new HashSet<Type>();
        }
		
		public BehaviorList For<TType>()
		{
			return For(typeof (TType));
		}

		public BehaviorList For(Type type)
		{
			var behaviorList = new BehaviorList();
			_behaviors[type] = behaviorList;
			return behaviorList;
		}
    }
}
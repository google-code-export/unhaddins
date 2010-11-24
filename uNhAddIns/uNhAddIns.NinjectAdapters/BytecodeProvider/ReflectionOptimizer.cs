using System;
using Ninject;
using NHibernate.Properties;


namespace uNhAddIns.NinjectAdapters.BytecodeProvider
{
    public class ReflectionOptimizer : NHibernate.Bytecode.Lightweight.ReflectionOptimizer
    {

        public ReflectionOptimizer(IKernel Kernel,
            Type mappedType, IGetter[] getters, ISetter[] setters)
            : base(mappedType, getters, setters)
        {
            kernel = Kernel;
        }

        protected IKernel kernel;

        /// <summary>
        /// Ignore this check
        /// </summary>
        /// <param name="type"></param>
        protected override void ThrowExceptionForNoDefaultCtor(Type type)
        {
        }

        public override object CreateInstance()
        {
                return kernel.Get(mappedType);
        }

    }
}

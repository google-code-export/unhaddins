//TODO don't like this ap
namespace uNhAddIns.PostSharpAdapters
{
    /// <summary>
    /// If the ServiceLocator can resolve this class, all the code to manage the conversation become ignored.
    /// </summary>
    public class NoopConversationalMarker
    {

		//this property is because Ninject and other containers 
		//can resolve a type that is not even registered
		//I don't like this approach.
		public bool Noop { get; set; }

    }
}
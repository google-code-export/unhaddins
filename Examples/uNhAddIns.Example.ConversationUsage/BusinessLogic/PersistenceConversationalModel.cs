using System;
using uNhAddIns.SessionEasier.Conversations;

namespace uNhAddIns.Example.ConversationUsage.BusinessLogic
{
	public enum ConversationEndMode
	{
		///<summary>
		/// Continue the conversation
		///</summary>
		Continue,
		///<summary>
		/// end and accept the changes
		///</summary>
		End,
		///<summary>
		/// end and abort the changes
		///</summary>
		Abort
	}

	[Serializable]
	public class PersistenceConversationalModel : IDisposable
	{
		private readonly IConversationsContainerAccessor cca;
		private readonly IConversationFactory cf;
		private string conversationId;

		protected PersistenceConversationalModel(IConversationsContainerAccessor cca, IConversationFactory cf)
		{
			if (cca == null)
			{
				throw new ArgumentNullException("cca");
			}
			if (cf == null)
			{
				throw new ArgumentNullException("cf");
			}
			this.cca = cca;
			this.cf = cf;
		}

		protected IDisposable GetConversationCaregiver()
		{
			return GetConversationCaregiver(ConversationEndMode.Continue);
		}

		protected IDisposable GetConversationCaregiver(ConversationEndMode endMode)
		{
			return new PersistenceConversationCaregiver(this, endMode);
		}

		protected void EndPersistenceConversation()
		{
			IConversation c = cca.Container.Get(GetConvesationId());
			if(c!=null)
			{
				c.Resume();
				c.End();
			}
		}

		protected void AbortPersistenceConversation()
		{
			IConversation c = cca.Container.Get(GetConvesationId());
			if (c != null)
			{
				c.Abort();
			}
		}

		protected virtual string GetConvesationId()
		{
			if (conversationId == null)
			{
				conversationId = Guid.NewGuid().ToString();
			}
			return conversationId;
		}

		protected virtual void ManageException(Exception e)
		{
			IConversation c = cca.Container.Unbind(conversationId);
			if (c != null)
			{
				c.Dispose();
			}
		}

		#region Implementation of IDisposable
		
		public void Dispose()
		{
			if (conversationId != null)
			{
				IConversation c = cca.Container.Get(conversationId);
				if (c != null)
				{
					cca.Container.SetAsCurrent(c);
					cca.Container.CurrentConversation.Abort();
				}
			}
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				IConversation c = cca.Container.Unbind(conversationId);
				if (c != null)
				{
					c.Dispose();
				}
			}
		}

		~PersistenceConversationalModel()
		{
			Dispose(false);
		}

		#endregion

		#region Nested type: PersistenceConversationCaregiver

		private class PersistenceConversationCaregiver : IDisposable
		{
			private readonly ConversationEndMode endMode;
			private readonly PersistenceConversationalModel pcm;

			public PersistenceConversationCaregiver(PersistenceConversationalModel pcm, ConversationEndMode endMode)
			{
				this.pcm = pcm;
				this.endMode = endMode;
				string convId = pcm.GetConvesationId();
				IConversation c = pcm.cca.Container.Get(convId) ?? pcm.cf.CreateConversation(convId);
				pcm.cca.Container.SetAsCurrent(c);
				c.Resume();
			}

			#region Implementation of IDisposable

			public void Dispose()
			{
				IConversation c = pcm.cca.Container.Get(pcm.GetConvesationId());
				switch (endMode)
				{
					case ConversationEndMode.End:
						c.End();
						break;
					case ConversationEndMode.Abort:
						c.Dispose();
						break;
					default:
						c.Pause();
						break;
				}
			}

			#endregion
		}

		#endregion
	}
}
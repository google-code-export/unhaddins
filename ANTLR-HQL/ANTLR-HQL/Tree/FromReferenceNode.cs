﻿using System;
using Antlr.Runtime;
using log4net;

namespace NHibernate.Hql.Ast.ANTLR.Tree
{
	public abstract class FromReferenceNode : AbstractSelectExpression, IResolvableNode, IDisplayableNode, IPathNode
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(FromReferenceNode));

		public static int ROOT_LEVEL = 0;
		private FromElement _fromElement;
		private bool _resolved;

		public FromReferenceNode(IToken token) : base(token)
		{
		}

		public override FromElement FromElement
		{
			get { return _fromElement; }
			set { _fromElement = value; }
		}

		public bool IsResolved
		{
			get { return _resolved; }
			set
			{
				_resolved = true;
				if (log.IsDebugEnabled)
				{
					log.Debug("Resolved :  " + Path + " -> " + Text);
				}
			}
		}

		public string Path
		{
			get { return getOriginalText(); }
		}


		public string GetDisplayText()
		{
			throw new NotImplementedException();
		}

		public abstract void Resolve(bool generateJoin, bool implicitJoin, string classAlias, IASTNode parent);

		public void Resolve(bool generateJoin, bool implicitJoin, string classAlias)
		{
			Resolve(generateJoin, implicitJoin, classAlias, null);
		}

		public void Resolve(bool generateJoin, bool implicitJoin)
		{
			Resolve(generateJoin, implicitJoin, null);
		}

		public void ResolveInFunctionCall(bool generateJoin, bool implicitJoin)
		{
			Resolve(generateJoin, implicitJoin, null);
		}

		public abstract void ResolveIndex(IASTNode parent);

		public string GetPath()
		{
			return getOriginalText();
		}

		public void RecursiveResolve(int level, bool impliedAtRoot, string classAlias, IASTNode parent)
		{
			IASTNode lhs = this.GetChild(0);
			int nextLevel = level + 1;
			if ( lhs != null ) 
			{
				FromReferenceNode n = ( FromReferenceNode ) lhs;
				n.RecursiveResolve( nextLevel, impliedAtRoot, null, this );
			}

			ResolveFirstChild();
			bool impliedJoin = true;

			if ( level == ROOT_LEVEL && !impliedAtRoot ) 
			{
				impliedJoin = false;
			}

			Resolve( true, impliedJoin, classAlias, parent );
		}

		/// <summary>
		/// Sub-classes can override this method if they produce implied joins (e.g. DotNode).
		/// </summary>
		/// <returns>an implied join created by this from reference.</returns>
		public virtual FromElement GetImpliedJoin()
		{
			return null;
		}

		public virtual void PrepareForDot(string propertyName)
		{
		}

		public virtual void ResolveFirstChild()
		{
		}
	}
}

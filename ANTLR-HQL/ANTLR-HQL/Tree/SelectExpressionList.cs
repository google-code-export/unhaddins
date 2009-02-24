﻿using System;
using System.Collections.Generic;
using Antlr.Runtime;

namespace NHibernate.Hql.Ast.ANTLR.Tree
{
	/// <summary>
	/// Common behavior - a node that contains a list of select expressions.
	/// Author: josh
	/// Ported by: Steve Strong
	/// </summary>
	public abstract class SelectExpressionList : HqlSqlWalkerNode 
	{
		protected SelectExpressionList(IToken token) : base(token)
		{
		}

		/// <summary>
		/// Returns an array of SelectExpressions gathered from the children of the given parent AST node.
		/// </summary>
		public ISelectExpression[] CollectSelectExpressions() 
		{
			// Get the first child to be considered.  Sub-classes may do this differently in order to skip nodes that
			// are not select expressions (e.g. DISTINCT).
			IASTNode firstChild = GetFirstSelectExpression();
			IASTNode parent = this;
			List<ISelectExpression> list = new List<ISelectExpression>(parent.ChildCount);

			for (int i = firstChild.ChildIndex; i < this.ChildCount; i++)
			{
				IASTNode n = GetChild(i);

				if ( n is ISelectExpression ) 
				{
					list.Add((ISelectExpression)n);
				}
				else 
				{
					throw new NotImplementedException();
					//throw new InvalidOperationException( "Unexpected AST: " + n.GetType().Name + " " + new ASTPrinter( SqlTokenTypes.)class ).showAsString( n, "" ) );
				}
			}
			return list.ToArray();
		}

		/// <summary>
		/// Returns the first select expression node that should be considered when building the array of select
		/// expressions.
		/// <summary>
		protected abstract IASTNode GetFirstSelectExpression();
	}
}

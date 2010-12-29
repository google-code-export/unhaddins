using System;
using System.ComponentModel;
using uNhAddIns.Entities;

namespace ChinookMediaManager.Domain
{
	public class Artist : Entity
	{
		public virtual string Name { get; set; }
	}
}
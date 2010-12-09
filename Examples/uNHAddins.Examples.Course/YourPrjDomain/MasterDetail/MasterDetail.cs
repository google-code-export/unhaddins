using System;
using System.Collections.Generic;

namespace YourPrjDomain.MasterDetail
{
	public class Master
	{
		private int _id;
		private string _name;
		private readonly IList<Detail> _details = new List<Detail>();

		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public virtual int Id
		{
			get { return _id; }
		}

		public virtual IEnumerable<Detail> Details
		{
			get { return _details; }
		}

		public virtual void AddDetail(Detail d)
		{
			if (d.Master != this)
				throw new Exception("No corresponde a este Master");
			_details.Add(d);
		}

		public virtual void RemoveDetail(Detail d)
		{
			_details.Remove(d);
		}
	}

	public class Detail
	{
		private int _id;
		private string _description;
		private readonly Master _master;

		protected Detail() {}
		public Detail(string _description, Master _master)
		{
			this._description = _description;
			this._master = _master;
		}

		public virtual string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		protected virtual int Id
		{
			get { return _id; }
		}

		public virtual Master Master
		{
			get { return _master; }
		}

		public override bool Equals(object obj)
		{
			Detail that = obj as Detail;
			if (that == null) return false;
			if (_id > 0)
				return _id == that._id;
			else
				return ReferenceEquals(this, that);
		}

		public override int GetHashCode()
		{
			if (_id > 0)
				return _id.GetHashCode();
			else
				return base.GetHashCode();
		}
	}
}

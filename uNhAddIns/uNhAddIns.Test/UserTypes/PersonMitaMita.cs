using System.Collections.ObjectModel;

namespace uNhAddIns.Test.UserTypes
{
    public class PersonMitaMita 
    {
        public virtual string Name { get; set; }
        public virtual Gender Gender { get; set; }
    }

    public class Gender
    {
        internal Gender(string id)
        {
            Id = id;
        }

        public string Id { get; protected set; }

        public string Name { get; set; }

        public bool Equals(Gender other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.Id == Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Gender);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class Genders : ReadOnlyCollection<Gender>
    {
        public static Gender Male = new Gender("M") { Name = "Male" };
        public static Gender Female = new Gender("F") { Name = "Female" };
        public Genders() : base(new[] { Male, Female }) { }
    }
}
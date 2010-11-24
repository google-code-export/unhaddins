using Iesi.Collections.Generic;

namespace uNhAddIns.Test.Listeners.AutoDirtyCheck
{
	public class ReptileBuilder
	{
		private string description;
		private float bodyTemperature;

		private ReptileBuilder() { }
		public static ReptileBuilder StartRecording()
		{
			return new ReptileBuilder();
		}

		public ReptileBuilder WithDescription(string desc)
		{
			description = desc;
			return this;
		}

		public ReptileBuilder WithBodyTemperature(float teperature)
		{
			bodyTemperature = teperature;
			return this;
		}

		public Reptile Build()
		{
			return new Reptile {Description = description, BodyTemperature = bodyTemperature};
		}
	}

	public class HumanBuilder
	{
		private string description;
		private string name;
		private string nickName;

		private HumanBuilder() { }
		public static HumanBuilder StartRecording()
		{
			return new HumanBuilder();
		}

		public HumanBuilder WithDescription(string desc)
		{
			description = desc;
			return this;
		}

		public HumanBuilder WithName(string name)
		{
			this.name = name;
			return this;
		}

		public HumanBuilder WithNickName(string nick)
		{
			nickName = nick;
			return this;
		}

		public Human Build()
		{
			return new Human { Description = description, Name = name, NickName = nickName};
		}
	}

	public class HumanFamilyBuilder
	{
		private Human father = HumanBuilder.StartRecording().WithDescription("Surname").WithName("Father Name").Build();
		private Human mother = HumanBuilder.StartRecording().WithDescription("Surname").WithName("Mother Name").Build();
		private readonly ISet<Human> children= new HashedSet<Human>();

		private HumanFamilyBuilder() { }

		public static HumanFamilyBuilder StartRecording()
		{
			return new HumanFamilyBuilder();
		}

		public HumanFamilyBuilder WithFather(Human father)
		{
			this.father = father;
			return this;
		}

		public HumanFamilyBuilder WithMother(Human mother)
		{
			this.mother = mother;
			return this;
		}

		public HumanFamilyBuilder WithChildren(int numberOfChildren)
		{
			for (int i = 1; i <= numberOfChildren; i++)
			{
				children.Add(HumanBuilder.StartRecording().WithDescription(father.Description).WithName("Child " + i).Build());
			}
			return this;
		}

		public Family<Human> Build()
		{
			return new Family<Human> { Father = father, Mother = mother, Children= children };
		}
	}

	public class ReptileFamilyBuilder
	{
		private Reptile father = ReptileBuilder.StartRecording().WithDescription("Reptile Father").Build();
		private Reptile mother = ReptileBuilder.StartRecording().WithDescription("Reptile Mother").Build();
		private readonly ISet<Reptile> children = new HashedSet<Reptile>();

		private ReptileFamilyBuilder() { }

		public static ReptileFamilyBuilder StartRecording()
		{
			return new ReptileFamilyBuilder();
		}

		public ReptileFamilyBuilder WithFather(Reptile father)
		{
			this.father = father;
			return this;
		}

		public ReptileFamilyBuilder WithMother(Reptile mother)
		{
			this.mother = mother;
			return this;
		}

		public ReptileFamilyBuilder WithChildren(int numberOfChildren)
		{
			for (int i = 1; i <= numberOfChildren; i++)
			{
				children.Add(ReptileBuilder.StartRecording().WithDescription("Child " + i).Build());
			}
			return this;
		}

		public Family<Reptile> Build()
		{
			return new Family<Reptile> { Father = father, Mother = mother, Children = children };
		}
	}
}
namespace uNHAddins.Examples.Course.Tests.Search
{
	using Lucene.Net.Analysis.Snowball;
	using Lucene.Net.Analysis.Standard;

	/// <summary>
	/// Creates an english analyzer that receives no parameters. Internally uses the Snowball analyzer with
	/// the EnglishStemmer
	/// </summary>
	public class EnglishAnalyzer : SnowballAnalyzer
	{
		public EnglishAnalyzer() : base("English", StandardAnalyzer.STOP_WORDS)
		{
		}
	}
}
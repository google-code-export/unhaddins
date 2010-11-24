using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Snowball;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Highlight;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using NHibernate;
using NHibernate.Expressions;
using NHibernate.Search;
using NHibernate.Search.Engine;
using NHibernate.Search.Impl;
using NHibernate.Search.Storage;
using NUnit.Framework;
using SpellChecker.Net.Search.Spell;
using YourPrjDomain.Search;
using Directory=Lucene.Net.Store.Directory;
using Token=Lucene.Net.Analysis.Token;

namespace uNHAddins.Examples.Course.Tests.Search
{
	using System.Diagnostics;

	[TestFixture]
	public class SearchFixture : TestCase
	{
		private Analyzer searchAnalyzer;
		private QueryParser queryParser;

		#region Search Configurations

		protected override IList Mappings
		{
			get
			{
				return new string[] { "Search.Book.hbm.xml" };
			}
		}

		protected override void Configure(NHibernate.Cfg.Configuration configuration)
		{
			configuration.SetProperty("hibernate.search.default.directory_provider", typeof(RAMDirectoryProvider).AssemblyQualifiedName);
			configuration.SetProperty(Environment.AnalyzerClass, typeof(EnglishAnalyzer).AssemblyQualifiedName);

			/*
			 * NOTE: In real applications the indexes should be stored in the file system (no in RAM), examples:
			// Example 1: Master Configuration
			
			configuration.SetProperty("hibernate.search.default.directory_provider", typeof(FSMasterDirectoryProvider).AssemblyQualifiedName);
			configuration.SetProperty("hibernate.search.default.sourceBase", "./lucenedirs/master/copy");
			configuration.SetProperty("hibernate.search.default.indexBase", "./lucenedirs/master/main");
			configuration.SetProperty("hibernate.search.default.refresh", "1"); //every minute

			// Example 2: Slave Configuration
			configuration.SetProperty("hibernate.search.default.directory_provider", typeof(FSSlaveDirectoryProvider).AssemblyQualifiedName);
			configuration.SetProperty("hibernate.search.default.sourceBase", "./lucenedirs/master/copy");
			configuration.SetProperty("hibernate.search.default.indexBase", "./lucenedirs/slave");
			configuration.SetProperty("hibernate.search.default.refresh", "1"); //every minute
			
			 * 
			 * For more information, please refer to:
			 * http://www.hibernate.org/hib_docs/search/reference/en/html/search-configuration.html
			 * 
			 */

			base.Configure(configuration);
		}

		protected override void BuildSessionFactory()
		{
			log.Debug("Building session factory");
			base.BuildSessionFactory();

			SearchFactory.Initialize(cfg, sessions);
		}

		#endregion

		protected override void OnSetUp()
		{
			using (new TemporaryOffLog(new string[] { "NHibernate.SQL", "NHibernate.Transaction" }))
			{
				// Build a sample catalog of books
				BuildBooksCatalog();

				// Create the search query and criterion (like the most common EqExpression, LikeExpression, and so on)
				//searchAnalyzer = new StandardAnalyzer();
				searchAnalyzer = new SnowballAnalyzer("English", StandardAnalyzer.STOP_WORDS);

				queryParser = new QueryParser("Summary", searchAnalyzer);
				queryParser.SetDefaultOperator(QueryParser.AND_OPERATOR);
			}
		}

		[Test]
		public void DoSearch()
		{
			log.Debug("Searching for 'nhibernate develop'");

			// Create the user query
			Query luceneQuery = queryParser.Parse("nhibernate develop");
			ICriterion query = NHibernate.Search.Search.Query(luceneQuery);

			Stopwatch watch = Stopwatch.StartNew();

			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession(new SearchInterceptor()))
			{
				// Create the search Criteria and do the search
				IList<Book> result = s.CreateCriteria(typeof(Book))
					.Add(query).List<Book>();

				log.Debug("Books found: " + result.Count);

				Assert.AreEqual(2, result.Count, "2 books contains 'nhibernate develop' in the summary");

				int index = 1;
				foreach (Book book in result)
				{
					// Display the book name (maybe you will need to create a link using the book id)
					log.DebugFormat("\t{0}. {1}", index, book.Name);

					// Search inside the book summary and decide which are the most important senteces
					Highlighter hl = new Highlighter(new SimpleHTMLFormatter(), new QueryScorer(luceneQuery));
					TokenStream tokenStream = searchAnalyzer.TokenStream("Summary", new StringReader(book.Summary));
					string bestFragment = hl.GetBestFragments(tokenStream, book.Summary, 2, "...");

					log.Debug("\t" + bestFragment);

					index++;
				}
			}

			log.Debug("Search done in " + watch.Elapsed.TotalSeconds + " seconds");
		}

		[Test]
		public void SpellingSuggestion()
		{
			log.Debug("Building the word index");

			// Create a "Did you mean?" dictionary (the words are extracted from the search index)
			Directory wordDirectory = GetWordDirectory();

			Directory wordIndex = new RAMDirectory();
			SpellChecker.Net.Search.Spell.SpellChecker checker = new SpellChecker.Net.Search.Spell.SpellChecker(wordIndex);
			checker.ClearIndex();

			IndexReader reader = IndexReader.Open(wordDirectory);
			// Add words to spell checker index
			checker.IndexDictionary(new LuceneDictionary(reader, SpellChecker.Net.Search.Spell.SpellChecker.F_WORD));

			// Suggest similar words
			SuggestAndVerify(checker, "nhibrenate", "nhibernate");
			SuggestAndVerify(checker, "dreiven", "driven");
			SuggestAndVerify(checker, "inyection", "injection");
		}

		private Directory GetWordDirectory()
		{
			/* Just for testing purposes, we will create the index using the information stored in the database
			 * We cannot use the already created index because we are using the SnowballAnalyzer, which cuts the words to what he
			 * thinks is the root form of the word. If we were using the StandardAnalyzer we would just do this:
			 * 
			 * return SearchFactory.GetSearchFactory(sessions).GetDirectoryProvider(typeof(Book)).Directory;
			 * 
			 * Anyway, the spell dictionary doesn't need to contain ALL the words existent in the documents. We can even 
			 * download an already built dictionary (like WordNet)
			 */

			Directory wordIndex = new RAMDirectory();
			IndexWriter indexWriter = new IndexWriter(wordIndex, new StandardAnalyzer(), true);

			using (new TemporaryOffLog("NHibernate.SQL"))
			using (ISession s = OpenSession())
			{
				// Get all books using NH as normal
				IList<Book> books = s.CreateCriteria(typeof(Book)).List<Book>();
				foreach (Book book in books)
				{
					// Create the documents with the fields we want to be indexed
					Document doc = new Document();
					doc.Add(new Field(SpellChecker.Net.Search.Spell.SpellChecker.F_WORD, new StringReader(book.Summary)));
					indexWriter.AddDocument(doc);
				}
			}

			indexWriter.Close();
			return wordIndex;
		}

		private static void SuggestAndVerify(SpellChecker.Net.Search.Spell.SpellChecker checker, string misspelledWord, string expectedSuggestion)
		{
			string[] similarWords = checker.SuggestSimilar(misspelledWord, 1);
			Assert.AreEqual(1, similarWords.Length);

			log.DebugFormat("If searching: '{0}'\t\t\t\tI suggest: '{1}'", misspelledWord, similarWords[0]);

			Assert.AreEqual(expectedSuggestion, similarWords[0]);
		}

		[Test]
		public void Analyzers()
		{
			string[] strings = new string[]
				{
					"The quick brown fox jumped over the lazy dogs",
					"XY&Z Corporation - xyz@example.com"
				};

			Analyzer[] analyzers = new Analyzer[]
				{
					new WhitespaceAnalyzer(),
					new SimpleAnalyzer(),
					new StopAnalyzer(),
					new StandardAnalyzer(),
					new SnowballAnalyzer("English", StopAnalyzer.ENGLISH_STOP_WORDS),	// Same as EnglishAnalyzer
					new KeywordAnalyzer()
				};

			foreach (string text in strings)
			{
				log.Debug("Analyzing \"" + text + "\"");

				// Make each analyzer analyze the current string.
				foreach (Analyzer analyzer in analyzers)
				{
					StringBuilder analysisText = new StringBuilder();
					analysisText.AppendLine("\t" + analyzer.GetType().Name + ":");
					analysisText.Append("\t\t");

					TokenStream stream = analyzer.TokenStream("contents", new StringReader(text));
					while (true)
					{
						Token token = stream.Next();
						if (token == null) break;

						analysisText.Append("[" + token.TermText() + "] ");
					}

					log.Debug(analysisText.ToString());
				}
			}
		}

		[Test]
		public void SnowballAnalyzer()
		{
			// The algorithm is language-specific, using stemming. Stemming algorithms attempt to reduce a word to a common root form.
			string text = "building build builds builded";
			string output = "Analyzing '" + text + "', generated the tokens: ";
			Dictionary<string, string> tokensFound = new Dictionary<string, string>();

			// Do the analyzis
			Analyzer analyzer = new SnowballAnalyzer("English", StandardAnalyzer.STOP_WORDS);
			TokenStream stream = analyzer.TokenStream("contents", new StringReader(text));
			while (true)
			{
				Token token = stream.Next();
				if (token == null)
					break;

				// Append only unique tokens
				if (!tokensFound.ContainsKey(token.TermText()))
				{
					tokensFound[token.TermText()] = token.TermText();
					output += "[" + token.TermText() + "] ";
				}
			}

			log.Debug(output);

			Assert.AreEqual(1, tokensFound.Count);
		}

		[Test]
		public void DisplayInternalIndex()
		{
			Directory mainIndexDir = SearchFactory.GetSearchFactory(sessions).GetDirectoryProvider(typeof(Book)).Directory;
			IndexReader reader = IndexReader.Open(mainIndexDir);
			TermEnum terms = reader.Terms();
			while (terms.Next())
			{
				Term term = terms.Term();
				log.Debug("In " + term.Field() + ": " + term.Text());
			}
		}

		[Test]
		public void Performance()
		{
			// The first time a query is executed, Lucene.NET executes some things that reduces the time.
			// So this test if intended for displaying the Lucene internal optimizations and caches

			using (new TemporaryOffLog("NHibernate.SQL"))
			{
				for (int i = 0; i < 50; i++)
				{
					Stopwatch watch = Stopwatch.StartNew();

					// Create the user query
					Query luceneQuery = queryParser.Parse("nhibernate develop");
					ICriterion query = NHibernate.Search.Search.Query(luceneQuery);

					using (ISession s = OpenSession(new SearchInterceptor()))
					{
						// Create the search Criteria and do the search
						IList<Book> result = s.CreateCriteria(typeof(Book))
							.Add(query).List<Book>();

						log.DebugFormat("Search done in {0:0.000} seconds ({1} books found)", watch.Elapsed.TotalSeconds, result.Count);
					}
				}
			}
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession(new SearchInterceptor()))
			{
				s.Delete("from Book b");
				s.Flush();
			}
		}

		#region Build Big Books Catalog with long summaries

		private void BuildBooksCatalog()
		{
			log.Debug("Creating sample books");

			using (new TemporaryOffLog("NHibernate.SQL"))
			using (IFullTextSession s = NHibernate.Search.Search.CreateFullTextSession(OpenSession(new SearchInterceptor())))
			using (ITransaction trans = s.BeginTransaction())
			{
				s.Save(new Book("NHibernate in Action", "Pierre Henri Kuaté",
								@"NHibernate practically exploded on the .NET scene. Why is this open-source tool so popular? Because it automates a tedious task: persisting your .NET objects to a relational database. The inevitable mismatch between your object-oriented code and the relational database requires you to write code that maps one to the other. This code is often complex, tedious and costly to develop. NHibernate does the mapping for you.
								Not only that, NHibernate makes it easy. Positioned as a layer between your application and your database, NHibernate takes care of loading and saving of objects. NHibernate applications are cheaper, more portable, and more resilient to change. And they perform better than anything you are likely to develop yourself.
								NHibernate in Action carefully explains the concepts you need, then gets you going. It builds on a single example to show you how to use NHibernate in practice, how to deal with concurrency and transactions, how to efficiently retrieve objects and use caching.
								The authors created NHibernate and they field questions from the NHibernate community every day–they know how to make NHibernate sing. Knowledge and insight seep out of every pore of this book.
								What's Inside
								- ORM concepts
								- Getting started
								- Many real-world tasks
								- The NHibernate application development process"));

				s.Save(new Book("Beginning Hibernate: From Novice to Professional", "Dave Minter",
								@"“[This] is a book about design in the .NET world, driven in an agile manner and infused with the products of the enterprise patterns community. [It] shows you how to begin applying such things as TDD, object relational mapping, and DDD to .NET projects...techniques that many developers think are the key to future software development.... As the technology gets more capable and sophisticated, it becomes more important to understand how to use it well. This book is a valuable step toward advancing that understanding.”
								– Martin Fowler, author of Refactoring and Patterns of Enterprise Application Architecture
								Patterns, Domain-Driven Design (DDD), and Test-Driven Development (TDD) enable architects and developers to create systems that are powerful, robust, and maintainable. Now, there’s a comprehensive, practical guide to leveraging all these techniques primarily in Microsoft .NET environments, but the discussions are just as useful for Java developers. 
								Drawing on seminal work by Martin Fowler (Patterns of Enterprise Application Architecture) and Eric Evans (Domain-Driven Design), Jimmy Nilsson shows how to create real-world architectures for any .NET application. Nilsson illuminates each principle with clear, well-annotated code examples based on C# 1.1 and 2.0. His examples and discussions will be valuable both to C# developers and those working with other .NET languages and any databases–even with other platforms, such as J2EE. Coverage includes 
								- Quick primers on patterns, TDD, and refactoring
								- Using architectural techniques to improve software quality
								- Using domain models to support business rules and validation
								- Applying enterprise patterns to provide persistence support via NHibernate
								- Planning effectively for the presentation layer and UI testing
								- Designing for Dependency Injection, Aspect Orientation, and other new paradigms."));

				s.Save(new Book("Applying Domain-Driven Design and Patterns: With Examples in C# and .NET", "Jimmy Nilsson",
								@"Beginning Hibernate is ideal if you're experienced in Java with databases (the traditional, or ""connected,"" approach), but are new to open source lightweight Hibernate&emdash;the most popular de facto object-relational mapping and database-oriented application development framework. This book packs in brand new information about the latest release of the Hibernate 3.2.x persistence layer and provides a clear introduction to the current standard for object-relational persistence in Java.
								Experienced author Dave Minter and contributor Jeff Linwood provide more in-depth examples than any other books for Hibernate beginners. The authors also present material in a lively, example-based manner&emdash;not in a dry, theoretical, hard-to-read fashion. And since the book keeps its focus on Hibernate without wasting time on nonessential third-party tools, you'll be able to immediately start building transaction-based engines and applications."));

				s.Save(new Book("Open Source .NET Development: Programming with NAnt, NUnit, NDoc, and More", "Brian Nantz",
								@"Perhaps the most revolutionary aspect of the arrival of Microsoft's .NET platform is the standardization of C# and the Common Language Runtime. Now, for the first time, programmers can develop and use open-source projects that are based on a language that is an international standard as well as compatible with both Microsoft and Linux platforms. 
								Open Source .NET Development is the definitive guide on .NET development in an open-source environment
								Inside, readers will find in-depth information on using NAnt, NDoc, NUnit, Draco.NET, log4net, and Aspell.Net with both Visual Studio .NET and the Mono Project. Brian Nantz not only shares the best open-source and ""free"" tools, frameworks, components, and products for .NET, he also provides usable, practical examples and projects. The result is a highly accessible reference for finding the tools that best fit your needs.

								Highlights include
								- An introduction to open source and its implementations of the .NET standards 
								- .NET development with open-source tools, including build automation, XML documentation, unit testing, continuous integration, and application logging 
								- A simple example of Integrating .NET open-source projects that integrates an Open Source SVG component with a System.Drawing graphical editor 
								- An Aspell.Net case study that shows the use of Draco.NET Continuous Integration in conjunction with NAnt, NUnit, NDoc, and the SharpDevelop IDE 
								- An exclusive look at ADO.NET database and ASP.NET Web development using PostgreSQL that runs on both Windows and Linux 
								- Appendixes on NAnt and NAntContrib tasks, log4netAppender configurations, and open-source security observations 
								- Whether you are a .NET developer interested in learning more about open-source tools or an open-source developer curious about .NET, this book will bridge the divide between these formerly distinct camps."));

				s.Save(new Book("Practical Software Factories in .NET", "Gunther Lenz",
								@"The promise of Software Factories is to streamline and automate software development-and thus to produce higher-quality software more efficiently. The key idea is to promote systematic reuse at all levels and exploit economies of scope, which translates into concrete savings in planning, development, and maintenance efforts. However, the theory behind Software Factories can be overwhelming, because it spans many disciplines of software development. On top of that, Software Factories typically require significant investments into reusable assets.
								This book was written in order to demystify the Software Factories paradigm by guiding you through a practical case study from the early conception phase of building a Software Factory to delivering a ready-made software product. The authors provide you with a hands-on example covering each of the four pillars of Software Factories: software product lines, architectural frameworks, model-driven development, and guidance in context.
								While the ideas behind Software Factories are platform independent, the Microsoft .NET platform, together with recent technologies such as DSL Tools and the Smart Client Baseline Architecture Toolkit, makes an ideal foundation. A study shows the different facets and caveats and demonstrates how each of these technologies becomes part of a comprehensive factory. Software Factories are a top candidate for revolutionizing software development. This book will give you a great starting point to understanding the concepts behind it and ultimately applying this knowledge to your own software projects.
								Contributions by Jack Greenfield, Wojtek Kozaczynski Foreword by Douglas C. Schmidt, Jack Greenfield, Jürgen Kazmeier and Eugenio Pace."));

				s.Save(new Book("LINQ in Action", "Fabrice Marguerie",
								@"LLINQ, Language INtegrated Query, is a new extension to the Visual Basic and C# programming languages designed to simplify data queries and database interaction. It addreses O/R mapping issues by making query operations like SQL statements part of the programming language. It also offers built-in support for querying in-memory collections like arrays or lists, XML, DataSets, and relational databases. 
								LINQ in Action is a fast-paced, comprehensive tutorial for professional developers. This book explores what can be done with LINQ, shows how it works in an application, and addresses the emerging best practices. It presents the general purpose query facilities offered by LINQ in the upcoming C# 3.0 and VB.NET 9.0 languages. A running example introduces basic LINQ concepts. You'll then learn to query unstructured data using LINQ to XML and relational data with LINQ to SQL. Finally, you'll see how to extend LINQ for custom applications. 
								LINQ in Action will guide you along as you explore this new world of lambda expressions, query operators, and expression trees. As well, you'll explore the new features of C# 3.0, VB.NET 9.0. The book is very practical, anchoring each new idea with running code. Whether you want to use LINQ to query objects, XML documents, or relational databases, you will find all the information you need to get started 
								But LINQ in Action does not stop at the basic code. This book also shows you how LINQ can be used for advanced processing of data, including coverage of LINQ's extensibility, which allows querying more data sources than those supported by default. All code samples are built on a concrete business case. The running example, LinqBooks, is a personal book cataloging system that shows you how to create LINQ applications with Visual Studio 2008."));

				s.Save(new Book("ASP.NET 3.5 Unleashed", "Stephen Walther",
								@"ASP.NET 3.5 Unleashed is the most comprehensive book available on the Microsoft ASP.NET 3.5 Framework, covering all aspects of the ASP.NET 3.5 Framework--no matter how advanced.
								This edition covers all the new features of ASP.NET 3.5. It explains Microsoft LINQ to SQL in detail. It includes a chapter on the two new data access controls introduced with the ASP.NET 3.5 Framework: ListView and DataPager. With its coverage of ASP.NET AJAX, this book shows you how to take advantage of Microsoft’s server-side AJAX framework to retrofit existing ASP.NET applications with AJAX functionality. It also demonstrates how to use Microsoft’s client-side AJAX framework to build the web applications of the future: pure client-side AJAX applications. All code samples are written in the C# programming language. (Visual Basic versions of all code samples are included on the CD-ROM that accompanies this book.)
								Take advantage of Microsoft’s new database query language, LINQ to SQL, to easily build database-driven web applications 
								Learn how to use the new ListView and DataPager data access controls to build flexible user interfaces 
								- Take advantage of ASP.NET AJAX when building both server-side and client-side web applications 
								- Use the AJAX Control Toolkit to create auto-complete text fields, draggable panels, masked edit fields, and complex animations 
								- Design ASP.NET websites 
								- Secure your ASP.NET applications 
								- Create custom components 
								- Build highly interactive websites that can scale to handle thousands of simultaneous users 
								- Learn to build a complete ASP.NET 3.5 website from start to finish–the last chapter of the book includes a sample ASP.NET 3.5 web application written with LINQ to SQL and ASP.NET AJAX
								CD-ROM includes all examples and source code presented in this book in both C# and Visual Basic."));

				trans.Commit();
			}
		}

		#endregion
	}
}
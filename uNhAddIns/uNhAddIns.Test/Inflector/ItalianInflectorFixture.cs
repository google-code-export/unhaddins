using NUnit.Framework;
using uNhAddIns.Inflector;

namespace uNhAddIns.Test.Inflector
{

    [TestFixture]
    public class ItalianInflectorFixture : BaseInflectorFixture
    {
        public class CustomInflector : ItalianInflector
        {
            public CustomInflector()
            {
                AddDataDictionary("OrdineCliente", "OrdiniClienti");
            }
        }

        public ItalianInflectorFixture()
        {
            SingularToPlural.Add("inglese", "inglesi");
            SingularToPlural.Add("prova", "prove");
            SingularToPlural.Add("veicolo", "veicoli");
            SingularToPlural.Add("dispositivo", "dispositivi");
            SingularToPlural.Add("posizione", "posizioni");            
            SingularToPlural.Add("attività", "attività");
            SingularToPlural.Add("intervento", "interventi");
            SingularToPlural.Add("personale", "personale");
            SingularToPlural.Add("referente", "referenti");
            SingularToPlural.Add("città", "città");
            SingularToPlural.Add("camicia", "camicie");
            SingularToPlural.Add("uomo", "uomini");
            SingularToPlural.Add("sacco", "sacchi");
            SingularToPlural.Add("lago", "laghi");
            SingularToPlural.Add("amico", "amici");

            SingularToPlural.Add("decreto", "decreti");

            SingularToPlural.Add("ricerca", "ricerche");
            SingularToPlural.Add("interruttore", "interruttori");
            SingularToPlural.Add("processo", "processi");
            SingularToPlural.Add("indirizzo", "indirizzi");

            SingularToPlural.Add("categoria", "categorie");
            SingularToPlural.Add("domanda", "domande");
            SingularToPlural.Add("abilità", "abilità");
            SingularToPlural.Add("agenzia", "agenzie");
            SingularToPlural.Add("film", "film");
            SingularToPlural.Add("archivio", "archivi");
            SingularToPlural.Add("indice", "indici");

            SingularToPlural.Add("automobile", "automobili");
            
            ClassNameToTableName.Add("Origini", "Origine");
            ClassNameToTableName.Add("OrdiniClienti", "OrdineCliente");
            ClassNameToTableName.Add("Fatture", "Fattura");

            AddClassNameToTableName("Attività", "Attività");
            AddClassNameToTableName("Telefono", "Telefoni");

            TestInflector = new CustomInflector();

        }

        private void AddClassNameToTableName(string singular, string plural)
        {
            ClassNameToTableName.Add(plural, singular);
        }
    }
}

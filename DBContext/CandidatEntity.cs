namespace JobPortal.DBContext
{
    public class CandidatEntity
    {
        public Guid Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Mail { get; set; }
        public string Telephone { get; set; }
        public string NiveauEtude { get; set; }
        public string NombreAnneesExperience { get; set; }
        public string DernierEmployeur { get; set; }
        public Guid OfferId { get; set; }
        public string CVFilePath { get; set; }


    }
}

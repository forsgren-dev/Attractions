namespace Models.DTO
{
    public class DbInfoDto
    {
        public int NrSeededUsers { get; set; } = 0;
        public int NrUnseededUsers { get; set; } = 0;
        public int NrSeededAttractions { get; set; } = 0;
        public int NrUnseededAttractions { get; set; } = 0;
        public int NrSeededAddresses { get; set; } = 0;
        public int NrUnseededAddresses { get; set; } = 0;
        public int NrSeededComments { get; set; } = 0;
        public int NrUnseededComments { get; set; } = 0;
        public int NrCities { get; set; } = 0;
        public int NrCountries { get; set; } = 0;
    }
}

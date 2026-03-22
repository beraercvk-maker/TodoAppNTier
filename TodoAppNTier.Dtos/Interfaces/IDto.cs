namespace TodoAppNTier.Business.Mappings.AutoMapper
{
  interface IDto
    {
        public int Id { get; set; }

        public string Definition { get; set; }

        public bool IsCompleted { get; set; }
    }
}
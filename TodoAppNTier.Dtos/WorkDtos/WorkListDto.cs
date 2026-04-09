namespace TodoAppNTier.Dtos.WorkDtos
{
    public class WorkListDto
    {
      public int AppUserId { get; set; }
      public int Id { get; set; }
        public string Definition { get; set; }
        public bool IsCompleted { get; set; }
    }
}
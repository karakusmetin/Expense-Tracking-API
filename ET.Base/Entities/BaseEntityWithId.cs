namespace ET.Base.Entities
{
    public abstract class BaseEntityWithId
    {
        public Guid Id { get; set; }
        public Guid InsertUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public Guid? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? DeleteUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public bool IsActive { get; set; }
    }
}

namespace EfCoreRelationships.Entities;

public class Character
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public Guid UserId { get; set; }
	public Guid WeaponId { get; set; }

	//Navigation Property
	public virtual ICollection<CharacterSkill> CharacterSkills { get; set; }
}

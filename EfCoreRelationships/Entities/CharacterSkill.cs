namespace EfCoreRelationships.Entities;

public class CharacterSkill
{
	public Guid Id { get; set; }
	public Guid CharacterId { get; set; }
	public Guid SkillId { get; set; }
}

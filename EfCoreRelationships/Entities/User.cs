namespace EfCoreRelationships.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Name { get; set; }

	//Navigation Property
	public virtual ICollection<Character> Characters { get; set; }
}

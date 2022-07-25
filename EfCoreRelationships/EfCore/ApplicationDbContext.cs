using EfCoreRelationships.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreRelationships.EfCore
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Character> Characters { get; set; }
		public DbSet<Skill> Skills { get; set; }
		public DbSet<CharacterSkill> CharacterSkills { get; set; }
		public DbSet<Weapon> Weapons { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/*
				FK tanımlı olan tabloda tanımlama yapmak daha doğru.
				Yani ters tabloda ya da collection bulunan yerde bağlama yapmamalıyız!
				Foreign key(FK) sahip...Id propertysi tanımlı olan entity tanım yapacağımız yerdir. 
			 
				Tasarladığımız ilişkiyi daha iyi anlamak adına;

				1 User n Character		-One to many
				1 Character 1 Weapon	-One to one
				n Character n Skill		-Many to many
			 */

			modelBuilder.Entity<Character>(b =>
			{
				//Character içerisinde bulunan UserId fk sını tanımladık.
				b.HasOne<User>()
				 //.WithMany() //Eğer Characters navigation property tanımlı değilse böyle kullanmak yeterli.
				 .WithMany(x => x.Characters) //With Navigation Property
				 .HasForeignKey(x => x.UserId)
				 .OnDelete(DeleteBehavior.Cascade);

				//Character içerisinde bulunan WeaponId fk sını tanımladık.
				b.HasOne<Weapon>()
				 .WithOne()
				 .HasForeignKey<Character>(x => x.WeaponId)
				 .OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<CharacterSkill>(b =>
			{
				//CharacterSkill içerisinde bulunan CharacterId fk sını tanımladık.
				b.HasOne<Character>()
				 //.WithMany() //Eğer CharacterSkills navitation property tanımlı değilse böyle kullanmak yeterli.
				 .WithMany(x => x.CharacterSkills) //With Navigation Property
				 .HasForeignKey(x => x.CharacterId)
				 .OnDelete(DeleteBehavior.Cascade);

				//CharacterSkill içerisinde bulunan SkillId fk sını tanımladık.
				b.HasOne<Skill>()
				 .WithMany()
				 .HasForeignKey(x => x.SkillId)
				 .OnDelete(DeleteBehavior.Cascade);
			});
		}
	}

	/*
	
	Daha fazla bilgi için; https://stackoverflow.com/questions/20886049/ef-code-first-foreign-key-without-navigation-property

	public class Parent
	{
		public int Id { get; set; }
	}
	public class Child
	{
		public int Id { get; set; }
		public int ParentId { get; set; }
	}

	Eğer yukarıdaki gibi bir tanımlama söz konusuysa fk bulunan entity için ilgili ayarları yapmak yeterli. Yani

	// Without referencing navigation properties (they're not there anyway)
    b.HasOne<Parent>()    // <---
        .WithMany()       // <---
        .HasForeignKey(c => c.ParentId);

	Fakat ekstradan navigation property de kullanılacaksa bunun da bağlanırken verilmesi zorunluluğu bulunmakta. Aksi halde 
	entity framework migration oluştururken belirlenen navigation property için ilişki bulamayacağından dolayı Child tablosunda
	"ParentId1" şeklinde bir foreign key oluşturabilir. 
	
	public class Parent
	{
		public int Id { get; set; }
		public ICollection<Child> Childs { get; set; } //Navigation Property
	}

	public class Child
	{
		public int Id { get; set; }
		public int ParentId { get; set; }
	}

	Bu durumu aşmak için;
	
	b.HasOne<Parent>()					// <---
		.WithMany(c => c.Childs)        // <---
		.HasForeignKey(c => c.ParentId);
	
	//https://stackoverflow.com/a/45088337
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			......

			builder.Entity<Parent>(b => {
				b.HasKey(p => p.Id);
				b.ToTable("Parent");
			});

			builder.Entity<Child>(b => {
				b.HasKey(c => c.Id);
				b.Property(c => c.ParentId).IsRequired();

				// Without referencing navigation properties (they're not there anyway)
				b.HasOne<Parent>()    // <---
					.WithMany()       // <---
					.HasForeignKey(c => c.ParentId);

				// Just for comparison, with navigation properties defined,
				// (let's say you call it Parent in the Child class and Children
				// collection in Parent class), you might have to configure them 
				// like:
				// b.HasOne(c => c.Parent)
				//     .WithMany(p => p.Children)
				//     .HasForeignKey(c => c.ParentId);

				b.ToTable("Child");
			});

			......
		}
	}
	*/
}

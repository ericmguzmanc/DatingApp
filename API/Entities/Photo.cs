using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    /* 
      ℹ️ This anotation makes sure that a db schema with name of photos is created 
      to materialize the Photo relation for an user, remember we didn't added 
      a dbset for photos, this relation would create this table with this annotation 
    */
    [Table("Photos")]
    public class Photo
    {
      public int Id { get; set; }
      public string Url { get; set; }
      public bool IsMain { get; set; }
      
      public string PublicId { get; set; }

      /* 
        ℹ️ Fully defined relation, where you add an entity and specify entity Id for a foreign key relation from one to many.
        By doing this, the AppUserId alwais is nullable:false and if the user related to the photos is deleted, 
        then the cascading efect deletes the photos for that user.
      */
      public AppUser AppUser { get; set; }
      public int AppUserId { get; set; }
    }
}
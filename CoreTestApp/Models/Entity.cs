using System.ComponentModel.DataAnnotations;

namespace CoreTestApp.Models
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }

        public Type GetEntityType()
        {
            var type = this.GetType();
            return type;
        }
    }
}

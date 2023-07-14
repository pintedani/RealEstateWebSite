using System.ComponentModel.DataAnnotations;

namespace Imobiliare.Entities
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

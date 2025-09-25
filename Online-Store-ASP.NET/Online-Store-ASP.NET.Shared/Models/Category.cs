using System.Collections.Generic;

namespace Online_Store_ASP.NET.Shared.Models
{
    /// <summary>
    /// Fields: Id, Name, Description, Products
    /// </summary>
    public class Category
    {
        /// <summary>
        /// Unique identifier (primary key).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Name of the category.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description of the category.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the products in this category.
        /// Many-to-Many relationship.
        /// </summary>
        public virtual ICollection<Product> Products { get; set; } = [];
    }
}

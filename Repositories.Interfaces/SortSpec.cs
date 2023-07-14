using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imobiliare.Repositories.Interfaces
{
  public class SortSpec
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SortSpec" /> class.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="sortDirection">The sort direction.</param>
    public SortSpec(string propertyName, SortDirection sortDirection)
    {
      this.PropertyName = propertyName;
      this.SortDirection = sortDirection;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SortSpec" /> class.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    public SortSpec(string propertyName) : this(propertyName, SortDirection.Ascending)
    {
    }

    /// <summary>
    /// Gets or sets the sort direction.
    /// </summary>
    /// <value>
    /// The sort direction.
    /// </value>
    public SortDirection SortDirection { get; set; }

    /// <summary>
    /// Gets or sets the name of the property to apply sorting on.
    /// </summary>
    /// <value>
    /// The name of the property.
    /// </value>
    public string PropertyName { get; set; }
  }

  public enum SortDirection
  {
    /// <summary>
    /// Ascending order
    /// </summary>
    Ascending,

    /// <summary>
    /// Descending order
    /// </summary>
    Descending
  }
}

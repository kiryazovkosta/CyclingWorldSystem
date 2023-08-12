// ------------------------------------------------------------------------------------------------
//  <copyright file="SortColumn.cs" company="Business Management System Ltd.">
//      Copyright "2023" (c), Business Management System Ltd.
//      All rights reserved.
//  </copyright>
//  <author>Kosta.Kiryazov</author>
// ------------------------------------------------------------------------------------------------

namespace Web.Models;

public class SortColumn
{
    public string Name { get; set; } = null!;
    public string Sort { get; set; } = null!;
    public string Icon { get; set; } = null!;
}

public class SortCollection
{
    private readonly Dictionary<string, SortColumn> columns = new Dictionary<string, SortColumn>();

    public void AddColumn(string name, string sort)
    {
        this.columns.TryAdd(name, new SortColumn() { Name = name, Sort = sort });
    }
    
    public void Update(string name, string sort, string icon)
    {
        if (this.columns.TryGetValue(name, out var column))
        {
            column.Sort = sort;
            column.Icon = icon;
        }
    }

    public SortColumn? Get(string name)
    {
        if (this.columns.TryGetValue(name, out var column))
        {
            return column;
        }

        return null;
    }
}
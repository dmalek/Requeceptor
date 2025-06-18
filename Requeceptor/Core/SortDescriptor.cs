namespace Requeceptor.Core;


/// <summary>
/// Sorting description
/// </summary>
/// <param name="Property"></param>
/// <param name="Order"></param>
public record SortDescriptor(
    string Property,
    SortOrder Order
    );

public enum SortOrder
{
    /// <summary>
    /// <b>Bez</b> sortiranja
    /// </summary>
    None,

    /// <summary>
    /// <b>Uzlazni</b> redosljed sortiranja
    /// </summary>
    Ascending,

    /// <summary>
    /// <b>Silazni</b> redosljed sortiranja
    /// </summary>
    Descending
}
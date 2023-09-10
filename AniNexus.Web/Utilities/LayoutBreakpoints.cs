namespace AniNexus.Web;

/// <summary>
/// Known breakpoint levels.
/// </summary>
public struct LayoutBreakpoints
{
    /// <summary>
    /// The default breakpoint.
    /// </summary>
    public ELayoutBreakpointColumnSize Default { get; set; }

    /// <summary>
    /// width &lt;= 768px
    /// </summary>
    public ELayoutBreakpointColumnSize Mobile { get; set; }

    /// <summary>
    /// 769px &lt;= width &lt;= 1023px
    /// </summary>
    public ELayoutBreakpointColumnSize Tablet { get; set; }

    /// <summary>
    /// 1024px &lt;= width &lt;= 1215px
    /// </summary>
    public ELayoutBreakpointColumnSize Desktop { get; set; }

    /// <summary>
    /// 1216px &lt;= width &lt;= 1407px
    /// </summary>
    public ELayoutBreakpointColumnSize Widescreen { get; set; }

    /// <summary>
    /// width &gt;= 1408px
    /// </summary>
    public ELayoutBreakpointColumnSize FullHD { get; set; }
}

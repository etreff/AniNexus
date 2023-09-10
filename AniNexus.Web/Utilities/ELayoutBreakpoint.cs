namespace AniNexus.Web;

/// <summary>
/// Known breakpoint levels.
/// </summary>
public enum ELayoutBreakpoint
{
    /// <summary>
    /// width &lt;= 768px
    /// </summary>
    Mobile,

    /// <summary>
    /// 769px &lt;= width &lt;= 1023px
    /// </summary>
    Tablet,

    /// <summary>
    /// 1024px &lt;= width &lt;= 1215px
    /// </summary>
    Desktop,

    /// <summary>
    /// 1216px &lt;= width &lt;= 1407px
    /// </summary>
    Widescreen,

    /// <summary>
    /// width &gt;= 1408px
    /// </summary>
    FullHD,

    /// <summary>
    /// The default breakpoint.
    /// </summary>
    Default = FullHD
}

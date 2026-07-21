namespace DeskFlow.Portfolio;

public sealed record ThemePalette(
    string Background,
    string Panel,
    string Border,
    string Accent,
    string AccentText);

public static class ThemeCatalog
{
    // Every palette keeps the same semantic roles, so contrast and hierarchy
    // remain predictable when the operator changes the visual theme.
    public static IReadOnlyDictionary<string, ThemePalette> All { get; } =
        new Dictionary<string, ThemePalette>
        {
            ["Graphite / Amber"] = new("#111214", "#1D1F23", "#34363D", "#F2B84B", "#FFE6A8"),
            ["Forest / Lime"] = new("#0E1510", "#18241B", "#2D4634", "#8FD14F", "#DDF7B9"),
            ["Plum / Orchid"] = new("#151017", "#24192A", "#44334D", "#D58AF0", "#F2D2FF"),
            ["Stone / Coral"] = new("#171311", "#25201C", "#4A3D36", "#FF856D", "#FFD3CB"),
            ["Midnight / Mint"] = new("#080F1D", "#101A2C", "#22324D", "#49DFBD", "#A9F1DD")
        };
}

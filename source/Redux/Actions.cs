using Redux;

namespace FeedReader.Redux
{
    internal class LoadAction : IAction { }

    internal class RefreshAction : IAction { }

    internal class HideAction : IAction { public string? Id { get; set; } }
}
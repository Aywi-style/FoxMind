namespace FoxMind.Code.Runtime.Plugins.Visitor
{
    public interface IVisitableItem<in TV>
    {
        void Accept(TV visitor);
    }
}
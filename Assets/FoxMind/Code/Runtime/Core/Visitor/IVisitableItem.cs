namespace FoxMind.Code.Runtime.Core.Visitor
{
    public interface IVisitableItem<in TV>
    {
        void Accept(TV visitor);
    }
}
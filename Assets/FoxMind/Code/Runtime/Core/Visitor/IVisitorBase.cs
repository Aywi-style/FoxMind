namespace FoxMind.Code.Runtime.Core.Visitor
{
    public interface IVisitorBase<in TI>
    {
        void Visit(TI item);
    }
}
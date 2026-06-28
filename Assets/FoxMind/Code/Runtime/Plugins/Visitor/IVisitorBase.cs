namespace FoxMind.Code.Runtime.Plugins.Visitor
{
    public interface IVisitorBase<in TI>
    {
        void UpdateVisit(TI item);
        void LateUpdateVisit(TI item);
        void FixedUpdateVisit(TI item);
    }
}
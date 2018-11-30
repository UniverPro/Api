using LinqKit;

namespace Uni.Infrastructure
{
    public static class Initializer
    {
        public static void Initialize()
        {
            LinqKitExtension.QueryOptimizer = ExpressionOptimizer.visit;
        }
    }
}

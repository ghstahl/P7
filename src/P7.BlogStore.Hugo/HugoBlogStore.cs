using P7.BlogStore.Core;

namespace P7.BlogStore.Hugo
{
    public class HugoBlogStore: HugoStoreBase<Blog>, IBlogStore
    {
        protected HugoBlogStore(IBiggyConfiguration biggyConfiguration) : base(biggyConfiguration)
        {
        }
    }
}

using System;
using System.Threading.Tasks;
using Hugo.Data.Json;
using P7.BlogStore.Core;
using P7.Store;

namespace P7.BlogStore.Hugo
{
    public class HugoBlogStore: HugoStoreBase<Blog>, IBlogStore
    {
        public HugoBlogStore(IBiggyConfiguration biggyConfiguration,
            ISorter<Blog> sorter) :
            base(biggyConfiguration,"blog", sorter)
        {
        }
    }
}

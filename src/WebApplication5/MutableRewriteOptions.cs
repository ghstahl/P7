using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Rewrite;

namespace WebApplication5
{
    public class MutableRewriteOptions : RewriteOptions
    {
        //
        // Summary:
        //     A list of Microsoft.AspNetCore.Rewrite.IRule that will be applied in order upon
        //     a request.
        public new IList<IRule> Rules { get; set; }
    }
}

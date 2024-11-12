using Microsoft.Extensions.Configuration;

namespace TunnelSoft.YSL {
    public class CompilerSettings {

        private readonly IConfiguration _configuration;

        public CompilerSettings(IConfiguration configuration) {
            _configuration = configuration;
        }

        public string CompileWritePath => (_configuration.GetSection("YSL:CompileWritePath").Get<string>()) ?? "/";


    }
}

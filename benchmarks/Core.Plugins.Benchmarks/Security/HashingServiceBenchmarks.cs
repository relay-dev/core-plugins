using BenchmarkDotNet.Attributes;
using Core.Plugins.Security;
using System;

namespace Core.Plugins.Benchmarks.Security
{
    [HtmlExporter]
    public class HashingServiceBenchmarks
    {
        private readonly Pbkdf2HashingService _pbkdf2HashingService;
        private readonly Sha256HashingService _sha256HashingService;

        public HashingServiceBenchmarks()
        {
            _pbkdf2HashingService = new Pbkdf2HashingService();
            _sha256HashingService = new Sha256HashingService();
        }

        [Benchmark]
        public string Sha256() => _sha256HashingService.CreateHash(Guid.NewGuid().ToString());

        [Benchmark]
        public string Pbkdf2() => _pbkdf2HashingService.CreateHash(Guid.NewGuid().ToString());
    }
}

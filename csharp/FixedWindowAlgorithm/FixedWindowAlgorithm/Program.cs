using System;
using System.Threading;
using System.Threading.Tasks;

namespace FixedWindowAlgorithm
{
    class Program
    {
        private static long _currentTime;
        private static long _current;
        private static Semaphore _semaphore = new Semaphore(0, 10);

        static void Main(string[] args)
        {
            _currentTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            _current = 0;
            for (int i = 0; i < 10; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 10000; j++)
                    {
                        FixedWindow();
                    }

                    _semaphore.Release(1);
                });
            }

            for (int i = 0; i < 10; i++)
            {
                _semaphore.WaitOne();
            }

            Console.WriteLine(_current);
            Console.WriteLine("sleep 2s");
            Thread.Sleep(2000);
            Console.WriteLine(FixedWindow());
        }

        public static long FixedWindow()
        {
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var ct = Interlocked.Read(ref _currentTime);
            if (now > ct)
            {
                if (Interlocked.CompareExchange(ref _currentTime, now, ct) == ct)
                {
                    Interlocked.Exchange(ref _current, 0);
                }
            }

            return Interlocked.Increment(ref _current);
        }
    }
}
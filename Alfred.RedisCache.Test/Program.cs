namespace Alfred.RedisCache.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RedisCacheTest redisCacheTest=new RedisCacheTest();

            //初始化
            redisCacheTest.Init();

            //测试
            //redisCacheTest.TestRedisSimple();

            redisCacheTest.TestRedisComple();

            Console.WriteLine("Hello World !!!");
            Console.ReadLine();
        }
    }
}
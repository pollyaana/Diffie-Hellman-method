using System.Numerics;

class Program
{
    static void Main()
    {
        BigInteger p = 0, n = 0;
        BigInteger q = RandomPrimeNum(BigPow(2, 255), BigPow(2, 256)); //256-битовое простое число (безопасный размер)
        while (!FermaTest(p))
        {
            n = RandomNum(BigPow(2,1023), BigPow(2,1024)); // согласно книге защита ~10 лет
            p = n * q + 1;
        }
        BigInteger g = GeneratorZ_p(p,n,q);
        BigInteger x = RandomNum(2, q - 2); // генерация секретного случайного числа собеседника А
        BigInteger y = RandomNum(2, q - 2); // генерация секретного случайного числа собеседника B
        BigInteger X = ModPow(g, x, p); // Абонент  А посылает абоненту В остаток от деления
        BigInteger Y = ModPow(g, y, p); // Абонент В посылает абоненту А остаток от деления
        BigInteger K_x = ModPow(Y, x, p); // Абонент А вычисляет ключ
        BigInteger K_y = ModPow(X, y, p); // Абонент В вычисляет ключ
        bool res = K_x == K_y;

        Console.WriteLine("Количество битов в p: " + Convert.ToString(BigInteger.Log(p + 1, 2)).Split(',')[0]);
        Console.WriteLine("Количество битов в q: " + Convert.ToString(BigInteger.Log(q + 1, 2)).Split(',')[0]);
        Console.WriteLine("Проверка совпадения ключей: " + res);
        Console.WriteLine("Секретный ключ:\n" + K_x);
    }

    static BigInteger BigPow(BigInteger a, BigInteger p)
    {
        BigInteger result = 1;
        while (p > 0)
        {
            if (p % 2 == 1)
                result *= a;
            p >>= 1;
            a *= a;
        }
        return result;
    }
    static BigInteger RandomNum(BigInteger min, BigInteger max)
    {
        Random random = new();
        var dif = (max - min).ToByteArray();
        random.NextBytes(dif);   
        return min + BigInteger.Abs(new BigInteger(dif)); 
    }

    static BigInteger RandomPrimeNum(BigInteger min, BigInteger max)
    {
        BigInteger q;
        while (true)
        {
            Random random = new();
            var dif = (max - min).ToByteArray();
            random.NextBytes(dif);
            q = min + BigInteger.Abs(new BigInteger(dif));
            if (FermaTest(q)) break;
        }
        return q;
    }
    static bool FermaTest(BigInteger n, int k=100)
    {
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        Random rand = new();
        var b = n.ToByteArray();
        rand.NextBytes(b);
        for (int i = 0; i < k; i++)
        {
            var a = new BigInteger(b);
            if (ModPow(a, n - 1, n) != 1)
                return false;
        }
        return true;
    }

    static BigInteger GeneratorZ_p(BigInteger p, BigInteger n, BigInteger q)
    {
        BigInteger g = 1;
        bool pr1, pr2;
        while (g == 1)
        {
            BigInteger a = RandomNum(1, p - 1);
            g = ModPow(a, n, p);
            pr1 = ModPow(g, q, p) != 1;
            pr2 = (p - 1) % q != 0;
            if (pr1 || pr2 )
                g = 1;
            else
            {
              Console.WriteLine
              ("Результат проверки того, что g^q mod p = 1: True");
              Console.WriteLine
              ("Результат проверки того, что q является делителем p-1: True");
            }
        }
        return g;
    }
    

    static BigInteger ModPow(BigInteger a, BigInteger b, BigInteger n)
    {
        BigInteger result = 1;
        a %= n;
        while (b > 0)
        {
            if (b % 2 == 1)
                result = result * a % n;
            b >>= 1;
            a = a * a % n;
        }
        return result;
    }
}
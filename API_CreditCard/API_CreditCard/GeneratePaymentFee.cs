using System;
namespace API_CreditCard
{
	public class GeneratePaymentFee
	{

        private static readonly Lazy<GeneratePaymentFee> instance = new Lazy<GeneratePaymentFee>(() => new GeneratePaymentFee());
        private static readonly object lockObject = new object();
        private readonly Random random;

        private GeneratePaymentFee()
		{
            random = new Random();
        }

        public static GeneratePaymentFee Instance
        {
            get
            {
                lock (lockObject)
                {
                    return instance.Value;
                }
            }
        }

        public decimal GenerateRandomDecimal()
        {
            lock (lockObject)
            {
                return (decimal)random.NextDouble() * 2;
            }
        }


    }
}



